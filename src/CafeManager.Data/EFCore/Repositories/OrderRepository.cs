using CafeManager.Core.Models.Orders;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common;

using EntityFramework.Exceptions.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CafeManager.Data.EFCore.Repositories;

public class OrderRepository(
    CafeManagerContext context, ILogger<OrderRepository> logger) :
    BaseRepository<Order, long>(context, logger),
    IOrderRepository
{
    protected override IQueryable<Order> IncludeAll(IQueryable<Order> queryable)
    {
        return queryable
            .Include(x => x.MenuItems);
    }

    public IAsyncEnumerable<Order> GetAsync(GetOrderFilter filter)
    {
        var query = IncludeAll(_context.Orders).AsNoTracking();

        query = filter.Status is not null
            ? query.Where(x => x.Status == filter.Status)
            : query;

        query = filter.From is not null
            ? query.Where(x => x.CompletedAt >= filter.From)
            : query;

        query = filter.To is not null
            ? query.Where(x => x.CompletedAt <= filter.To)
            : query;

        return query.AsAsyncEnumerable();
    }

    public override async Task<Order> CreateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.AttachRange(entity.MenuItems);
            _context.Orders.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return await IncludeAll(_context.Orders)
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

    public override async Task<Order> UpdateAsync(Order entity, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            try
            {
                _context.OrderMenuItems.Where(r => r.OrderId == entity.Id).ExecuteDelete();
                _context.AttachRange(entity.MenuItems);

                _context.Orders.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
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
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        await transaction.CommitAsync(cancellationToken);

        return await IncludeAll(_context.Orders)
                .AsNoTracking()
                .SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
    }
}