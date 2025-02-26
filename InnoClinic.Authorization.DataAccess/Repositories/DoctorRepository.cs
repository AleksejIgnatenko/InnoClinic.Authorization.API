using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models.DoctorModels;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public class DoctorRepository : RepositoryBase<DoctorEntity>, IDoctorRepository
    {
        public DoctorRepository(InnoClinicAuthorizationDbContext context) : base(context) { }

        public async Task<DoctorEntity> GetByIdAsync(Guid id)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.Id.Equals(id))
                ?? throw new DataRepositoryException($"Doctor with Id '{id}' not found.", StatusCodes.Status404NotFound); ;
        }

        public async Task<DoctorEntity> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Doctors
                .FirstOrDefaultAsync(d => d.Account.Id.Equals(accountId))
                ?? throw new DataRepositoryException($"Doctor with account Id '{accountId}' not found.", StatusCodes.Status404NotFound); ;
        }

        public override async Task UpdateAsync(DoctorEntity entity)
        {
            await _context.Doctors
                .Where(d => d.Id.Equals(entity.Id))
                .ExecuteUpdateAsync(d => d
                    .SetProperty(d => d.FirstName, entity.FirstName)
                    .SetProperty(d => d.LastName, entity.LastName)
                    .SetProperty(d => d.MiddleName, entity.MiddleName)
                    .SetProperty(d => d.CabinetNumber, entity.CabinetNumber)
                    .SetProperty(d => d.Status, entity.Status)
                );
        }
    }
}