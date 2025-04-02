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
        /// <exception cref="DataException">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId)
                ?? throw new DataException($"Account with Id '{accountId}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Retrieves a list of accounts by their identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>A list of <see cref="AccountEntity"/> objects associated with the specified identifiers.</returns>
        /// <exception cref="DataException">Thrown when no accounts are found for the provided identifiers.</exception>
        public async Task<List<AccountEntity>> GetByIdAsync(List<Guid> accountIds)
        {
            return await _context.Accounts
                .Where(a => accountIds.Contains(a.Id))
                .ToListAsync();
        }

        /// <summary>
        /// Checks if an email already exists in the accounts.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Email.Equals(email));
        }

        /// <summary>
        /// Retrieves an account by its email asynchronously.
        /// </summary>
        /// <param name="email">The email of the account.</param>
        /// <returns>The <see cref="AccountEntity"/> associated with the specified email.</returns>
        /// <exception cref="DataException">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByEmailAsync(string email)
        {
            return await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email.Equals(email))
                ?? throw new DataException($"Account with email '{email}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Updates an existing account asynchronously.
        /// </summary>
        /// <param name="entity">The account entity to update.</param>
        public override async Task UpdateAsync(AccountEntity entity)
        {
            var accountToUpdate = await GetByIdAsync(entity.Id);

            if (accountToUpdate != null)
            {
                accountToUpdate.Email = entity.Email;
                accountToUpdate.Password = entity.Password;
                accountToUpdate.RefreshToken = entity.RefreshToken;
                accountToUpdate.RefreshTokenExpiryTime = entity.RefreshTokenExpiryTime;
                accountToUpdate.IsEmailVerified = entity.IsEmailVerified;
                accountToUpdate.PhotoId = entity.PhotoId;
                accountToUpdate.CreateBy = entity.CreateBy;
                accountToUpdate.CreateAt = entity.CreateAt;
                accountToUpdate.UpdateBy = entity.UpdateBy;
                accountToUpdate.UpdateAt = entity.UpdateAt;

                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the phone number of an account asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        public async Task UpdatePhoneNumberAsync(Guid id, string phoneNumber)
        {
            var accountToUpdate = await GetByIdAsync(id);

            accountToUpdate.PhoneNumber = phoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePhotoAsync(Guid id, string photoId)
        {
            var accountToUpdate = await GetByIdAsync(id);

            accountToUpdate.PhotoId = photoId;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves an account by its refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token of the account.</param>
        /// <returns>The <see cref="AccountEntity"/> associated with the specified refresh token.</returns>
        /// <exception cref="DataException">Thrown when the account is not found.</exception>
        public async Task<AccountEntity> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken.Equals(refreshToken))
                ?? throw new DataException($"Account with refresh token '{refreshToken}' not found.", StatusCodes.Status404NotFound);
        }
    }
}
