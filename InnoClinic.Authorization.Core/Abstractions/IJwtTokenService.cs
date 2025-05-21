using System.Security.Claims;

namespace InnoClinic.Authorization.Core.Abstractions;

/// <summary>
/// Defines the operations for handling JSON Web Tokens (JWT).
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates an access token based on the provided claims.
    /// </summary>
    /// <param name="claims">The claims to include in the access token.</param>
    /// <returns>The generated access token as a string.</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>The generated refresh token as a string.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Extracts the account ID from the given access token.
    /// </summary>
    /// <param name="jwtToken">The JWT token from which to extract the account ID.</param>
    /// <returns>The account ID associated with the access token.</returns>
    Guid GetAccountIdFromAccessToken(string jwtToken);
}