namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides methods for password handling, including verification and hashing.
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Verifies the provided password against a hashed password.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>
        /// <c>true</c> if the password matches the hashed password; otherwise, <c>false</c>.
        /// </returns>
        bool Verify(string password, string hashedPassword);

        /// <summary>
        /// Generates a hashed representation of the provided password.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A string representing the hashed password.</returns>
        string GenerateHash(string password);
    }
}