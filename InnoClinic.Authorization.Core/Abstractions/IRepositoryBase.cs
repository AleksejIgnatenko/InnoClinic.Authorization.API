namespace InnoClinic.Authorization.DataAccess.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}