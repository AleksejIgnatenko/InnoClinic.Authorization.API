using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public class AccountRepository : RepositoryBase<AccountModel>, IAccountRepository
    {
        public AccountRepository(InnoClinicAuthorizationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return await _context.Accounts
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AccountModel> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == accountId)
                ?? throw new DataRepositoryException($"Account with Id '{accountId}' not found.", StatusCodes.Status404NotFound);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts
                .AnyAsync(a => a.Email.Equals(email));
        }

        public async Task<AccountModel> GetByEmail(string email)
        {
            return await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email.Equals(email))
                ?? throw new DataRepositoryException($"Account with Id '{email}' not found.", StatusCodes.Status404NotFound);

        }

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

        public async Task UpdateAsync(Guid id, string phoneNumber)
        {
            await _context.Accounts
                .Where(a => a.Id.Equals(id))
                .ExecuteUpdateAsync(a => a
                    .SetProperty (a => a.PhoneNumber, phoneNumber)
                );
        }

        public async Task<AccountModel> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.RefreshToken.Equals(refreshToken))
                ?? throw new DataRepositoryException($"Account with refresh token '{refreshToken}' not found.", StatusCodes.Status404NotFound);
        }
    }
}
