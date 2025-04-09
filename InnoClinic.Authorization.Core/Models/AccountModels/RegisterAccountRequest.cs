namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    /// <summary>
    /// Represents a request model for registering a new account.
    /// </summary>
    public record RegisterAccountRequest(
        /// <summary>
        /// The user's email address for account registration.
        /// </summary>
        string Email,

        /// <summary>
        /// The password chosen by the user for the account.
        /// </summary>
        string Password
    );
}