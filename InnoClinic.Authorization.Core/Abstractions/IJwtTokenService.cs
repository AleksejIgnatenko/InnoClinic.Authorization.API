using System.Security.Claims;

namespace InnoClinic.Authorization.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        Guid GetAccountIdFromAccessToken(string jwtToken);
    }
}