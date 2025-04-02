namespace InnoClinic.Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// Represents options for JSON Web Token (JWT) configuration.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Gets or sets the secret key used for signing the token.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the issuer of the token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the audience for which the token is intended.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time of the access token in minutes.
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the refresh token in days.
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
