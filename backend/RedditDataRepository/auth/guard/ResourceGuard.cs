using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http.Controllers;

namespace RedditDataRepository.auth.guard
{
    /// <summary>
    /// Provides functionality to guard resources based on JWT token authentication.
    /// </summary>
    public class ResourceGuard
    {
        /// <summary>
        /// Checks if the provided email matches the email extracted from the JWT token present in the request.
        /// </summary>
        /// <param name="actionContext">The context of the HTTP action being executed.</param>
        /// <param name="email">The email to compare with the email extracted from the JWT token.</param>
        /// <returns>True if the email matches; otherwise, false.</returns>
        /// <remarks>
        /// This method extracts the JWT token from the Authorization header of the HTTP request, 
        /// validates and decodes the token, and extracts the email claim from it. 
        /// It then compares the extracted email with the provided email. 
        /// If they match, it returns true; otherwise, it returns false.
        /// </remarks>
        public static bool RunCheck(HttpContext httpContext, string email)
        {
            try
            {
                // Get JWT token from the request
                string token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                // Validate and decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                // Extract email claim from JWT
                string jwtEmail = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

                // Check if the email matches the requested email
                if (jwtEmail != email)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                // Return false if an exception occurs during token processing
                return false;
            }
        }
    }
}
