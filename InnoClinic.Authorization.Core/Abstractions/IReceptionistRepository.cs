using InnoClinic.Authorization.Core.Models.ReceptionistModels;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public interface IReceptionistRepository : IRepositoryBase<ReceptionistEntity>
    {
        Task<ReceptionistEntity> GetByIdAsync(Guid id);
    }
}