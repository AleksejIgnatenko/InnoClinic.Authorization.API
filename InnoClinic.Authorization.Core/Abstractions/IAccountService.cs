using InnoClinic.Authorization.Core.Models.AccountModels;
using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Authorization.Application.Services
{
    /// <summary>
    /// Defines the operations for managing user accounts.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Asynchronously creates a new account with the specified email and password.
        /// </summary>
        /// <param name="email">The email address for the new account.</param>
        /// <param name="password">The password for the new account.</param>
        /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
        /// <returns>A tuple containing the access token, refresh token, and a message indicating the result.</returns>
        Task<(string accessToken, string refreshToken)> CreateAccountAsync(string email, string password, IUrlHelper urlHelper);

        /// <summary>
        /// Asynchronously logs in the user and generates tokens.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A tuple containing the hashed password, access token, and refresh token.</returns>
        Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password);

        /// <summary>
        /// Asynchronously refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token used to obtain a new access token.</param>
        /// <returns>A tuple containing the new access token and refresh token.</returns>
        Task<(string accessToken, string refreshToken)> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Asynchronously confirms the email associated with the specified account ID using a token.
        /// </summary>
        /// <param name="accountId">The ID of the account to confirm.</param>
        /// <param name="token">The token used to confirm the email.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if confirmation was successful.</returns>
        Task<bool> ConfirmEmailAsync(Guid accountId, string token);

        /// <summary>
        /// Asynchronously checks if the specified email already exists.
        /// </summary>
        /// <param name="email">The email address to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the email exists.</returns>
        Task<bool> IsEmailAvailableAsync(string email);

        /// <summary>
        /// Asynchronously retrieves all accounts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, returning a collection of account models.</returns>
        Task<IEnumerable<AccountEntity>> GetAllAccountsAsync();

        /// <summary>
        /// Gets an account by its ID asynchronously.
        /// </summary>
        /// <param name="token">The access token containing the account ID.</param>
        /// <returns>The account model corresponding to the provided ID.</returns>
        Task<AccountEntity> GetAccountByIdFromTokenAsync(string token);

        /// <summary>
        /// Retrieves a list of accounts by their unique identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>An <see cref="IEnumerable{AccountModel}"/> containing the accounts associated with the specified identifiers.</returns>
        Task<IEnumerable<AccountEntity>> GetAccountsByIdsAsync(List<Guid> accountIds);

        /// <summary>
        /// Updates the phone number and photo ID for the specified account.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="phone">The new phone number for the account.</param>
        /// <param name="photoId">The new photo ID for the account. This can be null.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdatePhonePhotoInAccountAsync(Guid id, string phone, string? photoId);

        Task<AccountEntity> GetAccountByIdAsync(Guid id);
        Task<AccountEntity> GetAccountByEmailAsync(string email);
    }
}