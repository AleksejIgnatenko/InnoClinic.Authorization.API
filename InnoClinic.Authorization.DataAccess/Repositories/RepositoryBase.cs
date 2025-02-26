using InnoClinic.Authorization.DataAccess.Context;

namespace InnoClinic.Authorization.DataAccess.Repositories
{
    /// <summary>
    /// Base repository class that provides common data operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly InnoClinicAuthorizationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RepositoryBase(InnoClinicAuthorizationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public virtual async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
