using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides services for generating and validating JWT tokens.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
        /// </summary>
        /// <param name="options">The JWT options containing configuration settings.</param>
        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Generates a JWT access token based on the provided claims.
        /// </summary>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>A string representing the generated access token.</returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.AccessTokenExpirationMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        /// <summary>
        /// Generates a new refresh token.
        /// </summary>
        /// <returns>A string representing the generated refresh token.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Extracts the account ID from the provided access token.
        /// </summary>
        /// <param name="jwtToken">The JWT access token.</param>
        /// <returns>The account ID as a <see cref="Guid"/>.</returns>
        public Guid GetAccountIdFromAccessToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            var accountId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            return Guid.Parse(accountId);
        }
    }
}
