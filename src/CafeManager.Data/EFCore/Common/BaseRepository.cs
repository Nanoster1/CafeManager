using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Exceptions;

using EntityFramework.Exceptions.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CafeManager.Data.EFCore.Common;

public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    protected readonly CafeManagerContext _context;
    protected readonly ILogger<BaseRepository<TEntity, TId>> _logger;

    protected BaseRepository(CafeManagerContext context, ILogger<BaseRepository<TEntity, TId>> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected virtual IQueryable<TEntity> IncludeAll(IQueryable<TEntity> queryable) => queryable;

    public virtual async Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await IncludeAll(_context.Set<TEntity>())
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken) ??
            throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} with id {id} not found.");
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return await IncludeAll(_context.Set<TEntity>())
                .AsNoTracking()
                .SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
        }
        catch (UniqueConstraintException ex)
        {
            throw GetUniqueConstraintException(ex);
        }
        catch (ReferenceConstraintException ex)
        {
            throw ReferenceConstraintException(ex);
        }
    }

    public virtual async Task DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>()
            .Where(x => x.Id.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return await IncludeAll(_context.Set<TEntity>())
                .AsNoTracking()
                .SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
        }
        catch (UniqueConstraintException ex)
        {
            throw GetUniqueConstraintException(ex);
        }
        catch (ReferenceConstraintException ex)
        {
            throw ReferenceConstraintException(ex);
        }
    }

    protected virtual Exception GetUniqueConstraintException(UniqueConstraintException ex)
    {
        _logger.LogWarning("Unique constraint exception: {ex}", ex);
        return new EntityConflictException($"Duplicated unique properties: {string.Join(", ", ex.ConstraintProperties)}");
    }

    protected virtual Exception ReferenceConstraintException(ReferenceConstraintException ex)
    {
        _logger.LogWarning("Reference constraint exception: {ex}", ex);
        return new EntityConflictException($"Invalid references: {string.Join(", ", ex.ConstraintProperties)}");
    }
}