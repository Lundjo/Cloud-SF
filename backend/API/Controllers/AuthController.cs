using API.Models.auth;
using DataRepository.auth.JWT.JWTBaseClass;
using DataRepository.auth.JWT.JWTStorage.KeyStorage;
using DataRepository.Blobs.Images;
using DataRepository.classes.Users;
using DataRepository.cloud.account;
using DataRepository.users.Create;
using DataRepository.users.Read;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("health")]
        public async Task<IActionResult> Check()
        {
            return await Task.FromResult(Ok());
        }


        #region JWT
        // Create a JWT instance
        private static readonly JWT _jwtTokenGenerator = new JWT(JWTKeyStorage.SecretKey, "RCA", "students");
        #endregion

        #region LOGIN
        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(Login user)
        {
            // Check if any data is entered
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // If user exists, generate token
            if (await CheckUserCredentials.RunCheck(AzureTableStorageCloudAccount.GetCloudTable("Users"), user.Email, user.Password))
                return Ok(new { token = _jwtTokenGenerator.GenerateToken(user.Email) });
            else
                return Unauthorized();
        }
        #endregion

        #region SIGN UP
        /// <summary>
        /// Signs up a new user.
        /// </summary>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp()
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

                new AzureTableStorageCloudAccount();

                // Upload file to Azure Blob Storage
                (bool success, string blobUrl) = await AzureBlobStorage.UploadFileToBlobStorage(file.OpenReadStream(), fileExtension, "images");

                // If image uploaded, get image URL in blob storage and put into user table
                if (!success)
                {
                    return BadRequest();
                }
                else
                {
                    // Access form data and put into user object
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
                        ImageBlobUrl = blobUrl,
                        RowKey = Request.Form["email"]
                    };


                    // Put user into table
                    bool insertResult = await InsertUser.Execute(AzureTableStorageCloudAccount.GetCloudTable("users"), user);

                    if (insertResult)
                    {
                        // User was successfully added to the table
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
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
