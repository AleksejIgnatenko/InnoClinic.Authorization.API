using InnoClinic.Authorization.Core.Models.DoctorModels;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public interface IDoctorRepository : IRepositoryBase<DoctorEntity>
    {
        Task<DoctorEntity> GetByIdAsync(Guid id);
        Task<DoctorEntity> GetByAccountIdAsync(Guid accountId);
    }
}