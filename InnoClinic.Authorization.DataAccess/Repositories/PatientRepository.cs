using InnoClinic.Appointments.Core.Models.PatientModels;
using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public class PatientRepository : RepositoryBase<PatientEntity>, IPatientRepository
    {
        public PatientRepository(InnoClinicAuthorizationDbContext context) : base(context) { }

        public async Task<PatientEntity> GetByIdAsync(Guid id)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new DataRepositoryException("Patient not found", 404);
        }

        public async Task<PatientEntity> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Patients
                .FirstOrDefaultAsync(p => p.Account.Id == accountId)
                ?? throw new DataRepositoryException("Patient not found", 404);
        }

        public override async Task UpdateAsync(PatientEntity entity)
        {
            await _context.Patients
                .Where(p => p.Id.Equals(entity.Id))
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.FirstName, entity.FirstName)
                    .SetProperty(p => p.LastName, entity.LastName)
                    .SetProperty(p => p.MiddleName, entity.MiddleName)
                );
        }

        public override async Task DeleteAsync(PatientEntity entity)
        {
            await _context.Patients
                .Where(p => p.Id.Equals(entity.Id))
                .ExecuteDeleteAsync();
        }
    }
}
