using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    /// <summary>
    /// Defines the operations for managing account data in the repository.
    /// </summary>
    public interface IAccountRepository : IRepositoryBase<AccountModel>
    {
        /// <summary>
        /// Asynchronously retrieves all accounts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, returning a collection of account models.</returns>
        Task<IEnumerable<AccountModel>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves an account by its ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, returning the account model if found.</returns>
        Task<AccountModel> GetByIdAsync(Guid accountId);

        /// <summary>
        /// Retrieves a list of accounts by their identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>A list of <see cref="AccountModel"/> objects associated with the specified identifiers.</returns>
        /// <exception cref="DataRepositoryException">Thrown when no accounts are found for the provided identifiers.</exception>
        Task<List<AccountModel>> GetByIdAsync(List<Guid> accountIds);

        /// <summary>
        /// Asynchronously checks if the specified email already exists in the repository.
        /// </summary>
        /// <param name="email">The email address to check for existence.</param>
        /// <returns>A task that represents the asynchronous operation, returning true if the email exists.</returns>
        Task<bool> EmailExistsAsync(string email);

        /// <summary>
        /// Asynchronously retrieves an account by its email address.
        /// </summary>
        /// <param name="email">The email address of the account to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, returning the account model if found.</returns>
        Task<AccountModel> GetByEmailAsync(string email);

        /// <summary>
        /// Asynchronously updates the phone number of an account.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="phoneNumber">The new phone number to set.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(Guid id, string phoneNumber);

        /// <summary>
        /// Asynchronously retrieves an account by its refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token associated with the account.</param>
        /// <returns>A task that represents the asynchronous operation, returning the account model if found.</returns>
        Task<AccountModel> GetByRefreshTokenAsync(string refreshToken);
    }
}