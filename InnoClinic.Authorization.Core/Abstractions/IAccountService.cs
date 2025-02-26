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
        Task<(string accessToken, string refreshToken, string message)> CreateAccountAsync(string email, string password, IUrlHelper urlHelper);

        /// <summary>
        /// Asynchronously logs in the user and generates tokens.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>A tuple containing the hashed password, access token, and refresh token.</returns>
        Task<(string hashPassword, string accessToken, string refreshToken)> LoginAsync(string email);

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
        Task<bool> EmailExistsAsync(string email);

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
        Task<AccountEntity> GetAccountByIdAsync(string token);

        /// <summary>
        /// Retrieves a list of accounts by their unique identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>An <see cref="IEnumerable{AccountModel}"/> containing the accounts associated with the specified identifiers.</returns>
        Task<IEnumerable<AccountEntity>> GetAccountsByIdsAsync(List<Guid> accountIds);
    }
}