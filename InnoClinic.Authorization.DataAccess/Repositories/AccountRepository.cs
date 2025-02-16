﻿using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing account-related data operations.
    /// </summary>
    public class AccountRepository : RepositoryBase<AccountModel>, IAccountRepository
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
        /// <returns>A collection of <see cref="AccountModel"/>.</returns>
        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves an account by its identifier asynchronously.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <returns>The <see cref="AccountModel"/> associated with the specified identifier.</returns>
        /// <exception cref="DataRepositoryException">Thrown when the account is not found.</exception>
        public async Task<AccountModel> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId)
                ?? throw new DataRepositoryException($"Account with Id '{accountId}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Retrieves a list of accounts by their identifiers asynchronously.
        /// </summary>
        /// <param name="accountIds">A list of unique identifiers for the accounts.</param>
        /// <returns>A list of <see cref="AccountModel"/> objects associated with the specified identifiers.</returns>
        /// <exception cref="DataRepositoryException">Thrown when no accounts are found for the provided identifiers.</exception>
        public async Task<List<AccountModel>> GetByIdAsync(List<Guid> accountIds)
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
        /// <returns>The <see cref="AccountModel"/> associated with the specified email.</returns>
        /// <exception cref="DataRepositoryException">Thrown when the account is not found.</exception>
        public async Task<AccountModel> GetByEmail(string email)
        {
            return await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email.Equals(email))
                ?? throw new DataRepositoryException($"Account with email '{email}' not found.", StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Updates an existing account asynchronously.
        /// </summary>
        /// <param name="entity">The account entity to update.</param>
        public override async Task UpdateAsync(AccountModel entity)
        {
            await _context.Accounts
                    .Where(a => a.Id.Equals(entity.Id))
                    .ExecuteUpdateAsync(a => a
                        .SetProperty(a => a.Email, entity.Email)
                        .SetProperty(a => a.Password, entity.Password)
                        .SetProperty(a => a.RefreshToken, entity.RefreshToken)
                        .SetProperty(a => a.RefreshTokenExpiryTime, entity.RefreshTokenExpiryTime)
                        .SetProperty(a => a.IsEmailVerified, entity.IsEmailVerified)
                        .SetProperty(a => a.PhotoId, entity.PhotoId)
                        .SetProperty(a => a.CreateBy, entity.CreateBy)
                        .SetProperty(a => a.CreateAt, entity.CreateAt)
                        .SetProperty(a => a.UpdateBy, entity.UpdateBy)
                        .SetProperty(a => a.UpdateAt, entity.UpdateAt)
                    );
        }

        /// <summary>
        /// Updates the phone number of an account asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <param name="phoneNumber">The new phone number.</param>
        public async Task UpdateAsync(Guid id, string phoneNumber)
        {
            await _context.Accounts
                .Where(a => a.Id.Equals(id))
                .ExecuteUpdateAsync(a => a
                    .SetProperty(a => a.PhoneNumber, phoneNumber)
                );
        }

        /// <summary>
        /// Retrieves an account by its refresh token asynchronously.
        /// </summary>
        /// <param name="refreshToken">The refresh token of the account.</param>
        /// <returns>The <see cref="AccountModel"/> associated with the specified refresh token.</returns>
        /// <exception cref="DataRepositoryException">Thrown when the account is not found.</exception>
        public async Task<AccountModel> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken.Equals(refreshToken))
                ?? throw new DataRepositoryException($"Account with refresh token '{refreshToken}' not found.", StatusCodes.Status404NotFound);
        }
    }
}
