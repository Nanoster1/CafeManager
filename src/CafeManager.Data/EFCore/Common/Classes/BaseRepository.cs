
using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore.Common.Classes;

public class BaseRepository<T, TId> : IBaseRepository<T, TId>
    where T : class, IEntity<TId>
    where TId : struct
{
    protected readonly CafeManagerContext _context;

    protected BaseRepository(CafeManagerContext context) => _context = context;

    protected virtual IQueryable<T> IncludeAll(IQueryable<T> queryable) => queryable;

    public async Task<T> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken) ??
            throw new EntityNotFoundException($"Entity {typeof(T).Name} with id {id} not found.");
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }
}