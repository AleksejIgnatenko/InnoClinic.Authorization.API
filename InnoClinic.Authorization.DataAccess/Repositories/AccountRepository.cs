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
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == accountId)
                ?? throw new DataRepositoryException($"Account with Id '{accountId}' not found.", StatusCodes.Status404NotFound);
        }
    }
}
