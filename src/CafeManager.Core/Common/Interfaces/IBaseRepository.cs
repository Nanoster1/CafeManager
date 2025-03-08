namespace CafeManager.Core.Common.Interfaces;

public interface IBaseRepository<TEntity, TId>
    where TEntity : IEntity<TId>
    where TId : struct
{
    Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
}