using Microsoft.AspNetCore.Mvc;
using DataRepository.auth.guard;
using DataRepository.Blobs.Images;
using DataRepository.classes.Users;
using DataRepository.cloud.account;
using DataRepository.users.Read;
using DataRepository.users.Update;
using System.Net;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        #region GET
        /// <summary>
        /// Retrieves a user by email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>
        /// HTTP 200 OK with the user data if found,
        /// HTTP 404 Not Found if the user is not found,
        /// or HTTP 401 Unauthorized if the request is not authorized.
        /// </returns>
        [HttpGet("{email}")]
        [JwtAuthenticationFilter]
        public async Task<IActionResult> Get(string email)
        {
            // Check if the request is authorized
            if (!ResourceGuard.RunCheck(HttpContext, email))
            {
                // Return unauthorized if the request is not authorized
                return Unauthorized();
            }

            // Retrieve the user from the data repository
            User user = await ReadUser.Run(AzureTableStorageCloudAccount.GetCloudTable("users"), "User", email);

            // Return the user data if found
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                // Return 404 Not Found if the user is not found
                return NotFound();
            }
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Updates user profile information.
        /// </summary>
        [HttpPost("update")]
        [JwtAuthenticationFilter]
        public async Task<IActionResult> Update()
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
                // Access profile picture
                var file = Request.Form.Files[0]; // Only one file is uploaded
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                // Get file extension
                var fileExtension = Path.GetExtension(fileName).ToLower();

                // Access form data and put into user object
                User user = new User
                {
                    PartitionKey = "User",
                    FirstName = Request.Form["firstName"],
                    LastName = Request.Form["lastName"],
                    Address = Request.Form["address"],
                    City = Request.Form["city"],
                    Country = Request.Form["country"],
                    Phone = Request.Form["phone"],
                    Email = Request.Form["email"],
                    Password = Request.Form["password"],
                    ImageBlobUrl = Request.Form["imageBlobUrl"],
                    RowKey = Request.Form["email"],
                    ETag = Request.Form["ETag"],
                    Timestamp = DateTime.UtcNow
                };

                var newImage = Request.Form["newImage"];
                // if new image not provided file will be null
                if (bool.Parse(newImage))
                {

                    // Upload file to Azure Blob Storage
                    (bool success, string blobUrl) = await AzureBlobStorage.UploadFileToBlobStorage(file.OpenReadStream(), fileExtension, "images");

                    // If image uploaded get image url in blob storage
                    // and put into user table
                    if (!success)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        // Save blob url to user 
                        user.ImageBlobUrl = blobUrl;

                        // Put user into table
                        bool insert_result = await UpdateUser.Execute(AzureTableStorageCloudAccount.GetCloudTable("users"), user);

                        if (insert_result)
                        {
                            // User was successfully added to the table
                            // Remove old image from blob storage
                            await AzureBlobStorage.RemoveFileFromBlobStorage(Request.Form["imageBlobUrl"]);

                            return Ok(); // Return 200 OK
                        }
                        else
                        {
                            // User was not successfully added to the table
                            // Delete the image associated with the user
                            await AzureBlobStorage.RemoveFileFromBlobStorage(user.ImageBlobUrl);

                            return BadRequest("User creation failed"); // Return 400 Bad Request with an error message
                        }
                    }
                }
                else
                {
                    // Put user into table
                    bool insert_result = await UpdateUser.Execute(AzureTableStorageCloudAccount.GetCloudTable("users"), user);

                    if (insert_result)
                    {
                        return Ok(); // Return 200 OK
                    }
                    else
                    {
                        return BadRequest("User information can't be update");
                    }
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
