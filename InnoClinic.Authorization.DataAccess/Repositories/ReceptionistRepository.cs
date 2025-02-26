using InnoClinic.Authorization.Core.Exceptions;
using InnoClinic.Authorization.Core.Models.ReceptionistModels;
using InnoClinic.Authorization.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public class ReceptionistRepository : RepositoryBase<ReceptionistEntity>, IReceptionistRepository
    {
        public ReceptionistRepository(InnoClinicAuthorizationDbContext context) : base(context)
        {
        }

        public async Task<ReceptionistEntity> GetByIdAsync(Guid id)
        {
            return await _context.Receptionists
                .Include(r => r.Account)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new DataRepositoryException("Receptionist not found", 404);
        }
    }
}
