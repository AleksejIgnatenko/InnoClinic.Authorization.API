namespace InnoClinic.Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// Represents options for JSON Web Token (JWT) configuration.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Gets or sets the secret key used for signing the JWT.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the issuer of the JWT, typically the authority that issues the token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the audience for whom the JWT is intended.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time in minutes for the access token.
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the expiration time in days for the refresh token.
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
