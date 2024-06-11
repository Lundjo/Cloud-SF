using API.Models.post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using DataRepository.Blobs.Images;
using DataRepository.classes.Comments;
using DataRepository.classes.Posts;
using DataRepository.cloud.account;
using DataRepository.posts.Create;
using System.Net;
using DataRepository.comments.Read;
using DataRepository.posts.Read;
using DataRepository.auth.guard;
using DataRepository.comments.Delete;
using DataRepository.posts.Delete;
using System.Web;
using DataRepository.posts.Update;
using DataRepository.users.Read;
using DataRepository.Posts.Read;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        #region CREATE
        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <returns>
        /// An <see cref="IHttpActionResult"/> representing the result of the operation.
        /// </returns>
        [HttpPost("create")]
        [JwtAuthenticationFilter] // Requires JWT authentication
        public async Task<IActionResult> Create()
        {
            if (!Request.ContentType.StartsWith("multipart/form-data"))
            {
                return StatusCode((int)HttpStatusCode.UnsupportedMediaType);
            }

            // Store form data locally
            var uploadsFolder = "C:/slike";
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            try
            {
                var form = await Request.ReadFormAsync();
                var id = Guid.NewGuid().ToString();

                Post post = new Post()
                {
                    Id = id,
                    PartitionKey = "Post",
                    Author = form["author"],
                    Title = form["title"],
                    Content = form["content"],
                    HasImage = Request.Form.Files.Any(),
                    RowKey = id
                };

                if (post.HasImage)
                {
                    // Access profile picture
                    var file = Request.Form.Files[0]; // Only one file is uploaded
                    var name = file.FileName;

                    // Just get extension of file name
                    var fileExtension = Path.GetExtension(name).ToLower(); // Get the file extension

                    // Upload file to Azure Blob Storage
                    using (var fileStream = file.OpenReadStream())
                    {
                        (bool success, string blobUrl) = await AzureBlobStorage.UploadFileToBlobStorage(fileStream, fileExtension, "images");

                        // If image uploaded get image url in blob storage
                        // and put into post table
                        if (!success)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            // Save blob url to post 
                            post.ImageBlobUrl = blobUrl;
                        }
                    }
                }
                else
                {
                    // Image is optional
                    post.ImageBlobUrl = "";
                }

                // Put post into table
                bool insert_result = await InsertPost.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), post);

                if (insert_result)
                {
                    // User was successfully added to the table
                    return Ok(post.Id); // Return 200 OK and post id
                }
                else
                {
                    // Post was not successfully added to the table
                    // Delete the image associated with the post
                    if (post.HasImage)
                    {
                        await AzureBlobStorage.RemoveFileFromBlobStorage(post.ImageBlobUrl);
                    }

                    return BadRequest("Post creation failed"); // Return 400 Bad Request with an error message
                }
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region GET
        /// <summary>
        /// Get a specific post by post id
        /// </summary>
        /// <returns>
        /// An <see cref="IHttpActionResult"/> representing the result of the operation with post object.
        /// </returns>
        [HttpGet("{postId}")]
        public async Task<IActionResult> Get(string postId)
        {
            try
            {
                if (postId == null || postId == "")
                {
                    return BadRequest();
                }

                List<Comment> comments = await ReadComments.Execute(AzureTableStorageCloudAccount.GetCloudTable("comments"), postId);
                Post post = await ReadPost.Run(AzureTableStorageCloudAccount.GetCloudTable("posts"), "Post", postId);

                if (post == null)
                {
                    return NotFound();
                }

                // Create DTO object of post with comments
                GetPost getPost = new GetPost(post.Id, post.Author, post.Title, post.Content, post.HasImage, post.ImageBlobUrl, comments.OrderByDescending(x => x.Timestamp).ToList());

                return Ok(getPost);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Deletes a post with the specified post ID.
        /// </summary>
        /// <param name="postId">The ID of the post to delete.</param>
        /// <returns>
        /// IHttpActionResult representing the result of the deletion operation.
        /// Returns Ok() if the post and its associated comments were successfully deleted.
        /// Returns Unauthorized() if the request is not authorized.
        /// Returns BadRequest() if the comments associated with the post could not be deleted.
        /// Returns NotFound() if the post to delete is not found.
        /// Returns InternalServerError() if an unexpected error occurs during the deletion process.
        /// </returns>
        [HttpDelete("{postId}")]
        [JwtAuthenticationFilter] // Requires JWT authentication
        public async Task<IActionResult> Delete(string postId)
        {
            try
            {
                // Retrieve the post author by ID
                string author = await ReadPostAuthor.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), postId);

                // Only the author of the post can delete it
                if (!ResourceGuard.RunCheck(HttpContext, author))
                {
                    // Return unauthorized if the request is not authorized
                    return Unauthorized();
                }

                // Delete the post
                bool deleteResult = await DeletePost.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), postId);

                if (deleteResult)
                {
                    // Delete all comments associated with the post
                    bool deleteCommentsResult = await RemoveComments.Execute(AzureTableStorageCloudAccount.GetCloudTable("comments"), postId);

                    if (deleteCommentsResult)
                    {
                        return Ok();
                    }
                    else
                    {
                        // Return BadRequest if comments associated with the post could not be deleted
                        return BadRequest();
                    }
                }
                else
                {
                    // Return NotFound if the post to delete is not found
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                // Return InternalServerError if an unexpected error occurs during the deletion process
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region GET POSTS

        /// <summary>
        /// Retrieves posts based on pagination parameters such as post ID, search keywords, sorting criteria, and time.
        /// </summary>
        /// <param name="postId">The ID of the post to start pagination from.</param>
        /// <param name="searchKeywords">Keywords to filter posts by.</param>
        /// <param name="sort">The sorting criterion for the posts.</param>
        /// <param name="time">The timestamp indicating the starting point for pagination.</param>
        /// <returns>
        /// An IHttpActionResult containing a list of posts retrieved based on pagination parameters if successful;
        /// otherwise, an InternalServerError result containing the encountered exception.
        /// </returns>
        [HttpGet("{postId}/{searchKeywords}/pagination/{sort}/{time}")]
        public async Task<IActionResult> Pagination(string postId, string searchKeywords, int sort, string time)
        {
            try
            {
                // Convert the provided timestamp string to a DateTimeOffset object
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(time));
                DateTime newtime = dateTimeOffset.UtcDateTime;

                // Initialize remaining number of posts to be retrieved and a list to store retrieved posts
                int remaining = 3;
                List<Post> posts = new List<Post>();

                // Perform pagination until remaining posts to retrieve is zero
                while (remaining > 0)
                {
                    // Retrieve posts based on pagination parameters
                    var currentPosts = await ReadPosts.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), postId, remaining, searchKeywords, sort, newtime);

                    // If no more posts are retrieved, break out of the pagination loop
                    if (currentPosts.Count == 0)
                    {
                        break;
                    }
                    else if (posts.Count <= remaining)
                    {
                        return Ok(currentPosts);
                    }

                    // Add retrieved posts to the list and update remaining count
                    posts.AddRange(currentPosts);
                    remaining -= currentPosts.Count();
                }

                // Return an OK result containing the list of retrieved posts
                return Ok(posts);
            }
            catch (Exception e)
            {
                // Return an InternalServerError result containing the encountered exception if an error occurs
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region GET USERS POSTS


        /// <summary>
        /// Retrieves posts by a single user based on pagination parameters such as post ID, search keywords, sorting criteria, and time.
        /// </summary>
        /// <param name="postId">The ID of the post to start pagination from.</param>
        /// <param name="searchKeywords">Keywords to filter posts by.</param>
        /// <param name="sort">The sorting criterion for the posts.</param>
        /// <param name="time">The timestamp indicating the starting point for pagination.</param>
        /// <param name="encodedEmail">Email address of user whose posts to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing a list of posts retrieved based on pagination parameters if successful;
        /// otherwise, an InternalServerError result containing the encountered exception.
        /// </returns>
        [HttpGet("{postId}/{searchKeywords}/userPosts/{sort}/{time}/{encodedEmail}")]
        [JwtAuthenticationFilter]
        public async Task<IActionResult> UserPosts(string postId, string searchKeywords, int sort, string time, string encodedEmail)
        {
            try
            {
                // Convert the provided timestamp string to a DateTimeOffset object
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(time));
                DateTime newtime = dateTimeOffset.UtcDateTime;

                // Initialize remaining number of posts to be retrieved and a list to store retrieved posts
                int remaining = 3;
                List<Post> posts = new List<Post>();

                // Perform pagination until remaining posts to retrieve is zero
                while (remaining > 0)
                {
                    // Retrieve posts based on pagination parameters and user
                    var currentPosts = await ReadUsersPosts.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), postId, remaining, searchKeywords, sort, newtime, HttpUtility.UrlDecode(encodedEmail));

                    // If no more posts are retrieved, break out of the pagination loop
                    if (currentPosts.Count == 0)
                    {
                        break;
                    }
                    else if (posts.Count <= remaining)
                    {
                        return Ok(currentPosts);
                    }

                    // Add retrieved posts to the list and update remaining count
                    posts.AddRange(currentPosts);
                    remaining -= currentPosts.Count();
                }

                // Return an OK result containing the list of retrieved posts
                return Ok(posts);
            }
            catch (Exception e)
            {
                // Return an InternalServerError result containing the encountered exception if an error occurs
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region SUBSCRIBE
        /// <summary>
        /// Subscribes a user to a post
        /// </summary>
        /// <param name="postId">The ID of the post</param>
        /// <param name="encodedEmail">Email of the user who wants to subscribe</param>
        /// <returns>
        /// IHttpActionResult representing the result of the insert subscription operation.
        /// Returns Ok() if post exists, user exists, and if operation is completed successfully.
        /// Returns Unauthorized() if the email from the token and email in the request don't match.
        /// Returns BadRequest() if the user with associated email is not found.
        /// Returns NotFound() if the post doesn't exist.
        /// Returns InternalServerError() if an unexpected error occurs during the subscription process.
        /// </returns>
        [HttpGet("{postId}/{encodedEmail}/subscribe")]
        [JwtAuthenticationFilter]
        public async Task<IActionResult> Subscribe(string postId, string encodedEmail)
        {
            try
            {
                // Check if post exists by checking for its author
                string author = await ReadPostAuthor.Execute(AzureTableStorageCloudAccount.GetCloudTable("posts"), postId);
                if (author == null)
                {
                    return NotFound();
                }
                // Check if user exists
                string email = HttpUtility.UrlDecode(encodedEmail);
                if (!(await IsUserExists.RunCheckAsync(AzureTableStorageCloudAccount.GetCloudTable("users"), email)))
                {
                    return BadRequest();
                }
                // Checking if user is who he represents he is
                if (!ResourceGuard.RunCheck(HttpContext, email))
                {
                    return Unauthorized();
                }
                // Add check is subscription already exists
                var already_subsrcibed = await ReadSubscriptions.Run(AzureTableStorageCloudAccount.GetCloudTable("subscriptions"), postId);

                // If it is already subkrajzed return none
                if (already_subsrcibed.FirstOrDefault(p => p == email) != null)
                    return BadRequest();

                // Adding subscription 
                var insertResult = await SubscribeToPost.Execute(AzureTableStorageCloudAccount.GetCloudTable("subscriptions"), postId, email);
                if (insertResult)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion
    }
}
