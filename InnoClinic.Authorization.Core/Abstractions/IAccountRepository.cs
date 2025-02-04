using InnoClinic.Authorization.Core.Models;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public interface IAccountRepository : IRepositoryBase<AccountModel>
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<AccountModel> GetByIdAsync(Guid accountId);
        Task<bool> EmailExistsAsync(string email);
        Task<AccountModel> GetByEmail(string email);
    }
}