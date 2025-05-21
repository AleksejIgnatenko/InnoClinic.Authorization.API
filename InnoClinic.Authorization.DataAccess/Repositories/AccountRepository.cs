using InnoClinic.Authorization.Core.Abstractions;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models.AccountModels;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing account-related data operations.
    /// </summary>
    public class AccountRepository : RepositoryBase<AccountEntity>, IAccountRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AccountRepository(InnoClinicAuthorizationDbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Retrieves all accounts asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="AccountEntity"/>.</returns>
        public async Task<IEnumerable<AccountEntity>> GetAllAsync()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves an account by its identifier asynchronously.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <returns>The <see cref="AccountEntity"/> associated with the specified identifier.</returns>
        /// <exception cref="ExceptionWithStatusCode">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId)
                ?? throw new ExceptionWithStatusCode($"Account with Id '{accountId}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Retrieves a list of accounts by their identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>A list of <see cref="AccountEntity"/> objects associated with the specified identifiers.</returns>
        /// <exception cref="ExceptionWithStatusCode">Thrown when no accounts are found for the provided identifiers.</exception>
        public async Task<List<AccountEntity>> GetByIdAsync(List<Guid> accountIds)
        {
            return await _context.Accounts
                .AsNoTracking()
                .Where(a => accountIds.Contains(a.Id))
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an email already exists in the accounts.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _context.Accounts
                .AnyAsync(a => a.Email.Equals(email));
        }

        /// <summary>
        /// Retrieves an account by its email asynchronously.
        /// </summary>
        /// <param name="email">The email of the account.</param>
        /// <returns>The <see cref="AccountEntity"/> associated with the specified email.</returns>
        /// <exception cref="ExceptionWithStatusCode">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email.Equals(email))
                ?? throw new ExceptionWithStatusCode($"Account with email '{email}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Retrieves an account by its refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token of the account.</param>
        /// <returns>The <see cref="AccountEntity"/> associated with the specified refresh token.</returns>
        /// <exception cref="ExceptionWithStatusCode">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken.Equals(refreshToken))
                ?? throw new ExceptionWithStatusCode($"Account with refresh token '{refreshToken}' not found.", StatusCodes.Status404NotFound);
        }
    }
}
