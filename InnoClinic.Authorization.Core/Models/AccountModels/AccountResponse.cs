namespace InnoClinic.Authorization.Core.Models.AccountModels
{
    /// <summary>
    /// Represents a response model containing details about an account.
    /// </summary>
    public record AccountResponse(
        /// <summary>
        /// The unique identifier of the account.
        /// </summary>
        Guid Id,

        /// <summary>
        /// The email address associated with the account.
        /// </summary>
        string Email,

        /// <summary>
        /// The password for the account, typically not exposed in responses.
        /// </summary>
        string Password,

        /// <summary>
        /// The phone number associated with the account.
        /// </summary>
        string PhoneNumber,

        /// <summary>
        /// The role assigned to the account, indicating permissions.
        /// </summary>
        string Role,

        /// <summary>
        /// Indicates whether the email address has been verified.
        /// </summary>
        bool IsEmailVerified,

        /// <summary>
        /// The identifier of the user's profile photo, if one exists.
        /// </summary>
        string? PhotoId,

        /// <summary>
        /// The identifier of the user who created the account.
        /// </summary>
        string CreateBy,

        /// <summary>
        /// The date and time when the account was created.
        /// </summary>
        DateTime CreateAt,

        /// <summary>
        /// The identifier of the user who last updated the account.
        /// </summary>
        string UpdateBy,

        /// <summary>
        /// The date and time when the account was last updated.
        /// </summary>
        DateTime UpdateAt
    );
}