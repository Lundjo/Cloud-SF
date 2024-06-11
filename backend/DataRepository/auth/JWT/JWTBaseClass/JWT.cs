using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataRepository.auth.JWT.JWTBaseClass
{
    /// <summary>
    /// Provides functionality to generate JSON Web Tokens (JWT) for authentication purposes.
    /// </summary>
    public class JWT
    {
        private static string _secretKey;
        private static string _issuer;
        private static string _audience;

        /// <summary>
        /// Initializes a new instance of the JWT class.
        /// </summary>
        /// <param name="secretKey">The secret key used for token generation.</param>
        /// <param name="issuer">The issuer of the token.</param>
        /// <param name="audience">The audience of the token.</param>
        public JWT(string secretKey, string issuer, string audience)
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        /// <summary>
        /// Generates a JWT token with the provided email and expiration time.
        /// </summary>
        /// <param name="email">The email associated with the token.</param>
        /// <param name="expirationMinutes">Optional. The expiration time of the token in minutes. Default is 30 minutes.</param>
        /// <returns>The generated JWT token.</returns>
        public string GenerateToken(string email, int expirationMinutes = 30)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Email, email),
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
