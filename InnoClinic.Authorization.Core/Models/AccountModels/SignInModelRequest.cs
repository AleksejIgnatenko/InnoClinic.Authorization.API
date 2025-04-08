namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    /// <summary>
    /// Represents a request model for signing into an account.
    /// </summary>
    public record SignInModelRequest(
        /// <summary>
        /// The user's email address.
        /// </summary>
        string Email,

        /// <summary>
        /// The user's password.
        /// </summary>
        string Password);
}