using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RedditDataRepository.blobs.images;
using RedditDataRepository.classes.Posts;
using RedditDataRepository.cloud.account;
using RedditDataRepository.posts.Create;
using System.Net;

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
    }
}
