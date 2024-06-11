using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

/// <summary>
/// Represents an authentication filter for JWT (JSON Web Token) authorization in ASP.NET Web API.
/// </summary>
public class JwtAuthenticationFilter : AuthorizeAttribute
{
    /// <summary>
    /// Overrides the default authorization logic to implement JWT token-based authentication.
    /// </summary>
    /// <param name="actionContext">The context for the action to be authorized.</param>
    public override void OnAuthorization(HttpActionContext actionContext)
    {
        if (AuthorizeRequest(actionContext))
        {
            return;
        }

        HandleUnauthorizedRequest(actionContext);
    }

    /// <summary>
    /// Handles unauthorized requests by returning an HTTP unauthorized response.
    /// </summary>
    /// <param name="actionContext">The context for the action that triggered the unauthorized request.</param>
    protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
    {
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Authorizes the incoming HTTP request by validating the JWT token.
    /// </summary>
    /// <param name="actionContext">The context for the action to be authorized.</param>
    /// <returns>True if the request is authorized, otherwise false.</returns>
    private bool AuthorizeRequest(HttpActionContext actionContext)
    {
        try
        {
            var headers = actionContext.Request.Headers;
            if (!headers.Contains("Authorization"))
            {
                return false;
            }

            var tokenString = headers.GetValues("Authorization").FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(tokenString))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            // Check if the token has expired
            if (token.ValidTo < DateTime.UtcNow)
            {
                return false;
            }

            var principal = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "jwt"));

            Thread.CurrentPrincipal = principal;

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

