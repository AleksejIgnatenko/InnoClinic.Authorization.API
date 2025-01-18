using AutoMapper;
using InnoClinic.Authorization.Core.Models;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public class AccountRepository : RepositoryBase<AccountModel>, IAccountRepository
    {
        private readonly IMapper _mapper;
        public AccountRepository(InnoClinicAuthorizationDbContext context, IMapper mapper)
            : base(context) 
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();        }

        public async Task<AccountModel?> GetByIdAsync(Guid accountId)
        {
            return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);
        }
    }
}
