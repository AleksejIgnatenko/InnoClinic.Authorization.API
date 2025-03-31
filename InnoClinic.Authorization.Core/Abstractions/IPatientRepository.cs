using InnoClinic.Appointments.Core.Models.PatientModels;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public interface IPatientRepository : IRepositoryBase<PatientEntity>
    {
        Task DeleteAsync(PatientEntity entity);
        Task<PatientEntity> GetByAccountIdAsync(Guid accountId);
        Task<PatientEntity> GetByIdAsync(Guid id);
        Task UpdateAsync(PatientEntity entity);
    }
}