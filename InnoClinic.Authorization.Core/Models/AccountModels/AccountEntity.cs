using InnoClinic.Authorization.Core.Enums;

namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    public class AccountEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the email address associated with the account.
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

        /// <summary>
        /// Gets or sets the refresh token used for authentication.
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time of the refresh token.
        /// </summary>
        public DateTime RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email address has been verified.
        /// </summary>
        public bool IsEmailVerified { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the photo associated with the account.
        /// </summary>
        public string? PhotoId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the account was created by the user.
        /// </summary>
        public string CreateBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the account was created.
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the account was last updated by the user.
        /// </summary>
        public string UpdateBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the account was last updated.
        /// </summary>
        public DateTime UpdateAt { get; set; }
    }
}
