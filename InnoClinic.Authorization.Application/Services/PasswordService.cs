namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Provides methods for handling password operations, including verification and hashing.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Verifies a plain text password against a hashed password.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare with.</param>
        /// <returns><c>true</c> if the passwords match; otherwise, <c>false</c>.</returns>
        public bool Verify(string password, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        /// <summary>
        /// Generates a hashed representation of a plain text password.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>A string representing the hashed password.</returns>
        public string GenerateHash(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password, 10);
    }
}