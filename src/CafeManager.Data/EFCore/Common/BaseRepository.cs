using CafeManager.Core.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore.Common;

public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    protected readonly CafeManagerContext _context;

    protected BaseRepository(CafeManagerContext context) => _context = context;

    protected virtual IQueryable<TEntity> IncludeAll(IQueryable<TEntity> queryable) => queryable;

    public virtual async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await IncludeAll(_context.Set<TEntity>()).SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return await IncludeAll(_context.Set<TEntity>()).SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return await IncludeAll(_context.Set<TEntity>()).SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
    }
}