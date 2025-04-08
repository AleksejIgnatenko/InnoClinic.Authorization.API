using InnoClinic.Authorization.Application.Services;
using InnoClinic.Authorization.Core.Enums;
using InnoClinic.Authorization.Infrastructure.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace InnoClinic.Authorization.TestSuiteNUnit.ServiceTests
{
    public class JwtTokenServiceTests
    {
        private JwtTokenService _jwtTokenService;
        private List<Claim> _claimsExample;

        [SetUp]
        public void Setup()
        {
            var jwtOptions = Options.Create(new JwtOptions
            {
                SecretKey = "secretkeysecretkeysecretkeysecretkeysecretkeysecretkeysecretkey",
                Issuer = "InnoClinicAuthorizedIssuer",
                Audience = "InnoClinicAuthorizedAudience",
                AccessTokenExpirationMinutes = 15,
                RefreshTokenExpirationDays = 180,

            });
            _jwtTokenService = new JwtTokenService(jwtOptions);

            _claimsExample = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, RoleEnum.Receptionist.ToString()),
            };
        }

        [Test]
        public void GenerateAccessToken_ReturnsValidToken()
        {
            // Arrange
            var claims = _claimsExample;

            // Act
            var token = _jwtTokenService.GenerateAccessToken(claims);

            // Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

        [Test]
        public void GenerateRefreshToken_ReturnsValidToken()
        {
            // Act
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Assert
            Assert.IsNotNull(refreshToken);
            Assert.IsNotEmpty(refreshToken);
        }

        [Test]
        public void GetAccountIdFromAccessToken_ReturnsAccountId()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                new Claim(ClaimTypes.Role, RoleEnum.Receptionist.ToString()),
            };

            var token = _jwtTokenService.GenerateAccessToken(claims);

            // Act
            var extractedAccountId = _jwtTokenService.GetAccountIdFromAccessToken(token);

            // Assert
            Assert.AreEqual(accountId, extractedAccountId);
        }

        [Test]
        public void GetAccountIdFromAccessToken_InvalidToken_ThrowsException()
        {
            // Arrange
            var invalidToken = "invalidJwtToken";

            // Act and Assert
            Assert.Throws<SecurityTokenMalformedException>(() => _jwtTokenService.GetAccountIdFromAccessToken(invalidToken));
        }

        [Test]
        public void GetAccountIdFromAccessToken_MissingAccountId_ThrowsException()
        {
            // Arrange
            var tokenWithoutId = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InRlc3QiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJSZWNlcHRpb25pc3QiLCJleHAiOjE3NDAwMDQ0MzZ9.p3jdv-tiq_PlnvsSIqGUxSVozUE9KltS_LId34G9Uvk";

            // Act and Assert
            Assert.Throws<FormatException>(() => _jwtTokenService.GetAccountIdFromAccessToken(tokenWithoutId));
        }


    }
}
