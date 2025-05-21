namespace InnoClinic.Authorization.Core.Abstractions;

/// <summary>
/// Defines the basic operations for a repository pattern.
/// </summary>
/// <typeparam name="T">The type of the entity that the repository will manage.</typeparam>
public interface IRepositoryBase<T>
{
    /// <summary>
    /// Asynchronously creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAsync(T entity);

    /// <summary>
    /// Asynchronously updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Asynchronously deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(T entity);
}