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
    }
}
