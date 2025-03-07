namespace CafeManager.Core.Common.Interfaces;

public interface IBaseRepository<T, TId>
    where T : IEntity<TId>
    where TId : struct
{
    Task<T> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}