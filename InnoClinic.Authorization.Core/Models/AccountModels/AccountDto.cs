using InnoClinic.Authorization.Core.Enums;

namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    /// <summary>
    /// Represents the data transfer object for an account.
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account.
        /// </summary>
        public Guid Id { get; set; }
        public string PhotoId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email associated with the account.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the account.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number associated with the account.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the role associated with the account.
        /// Represents the access level or permissions of the user.
        /// </summary>
        public RoleEnum Role { get; set; }
    }
}
