using CafeManager.Core.Models.Orders;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore.Repositories;

public class OrderRepository(CafeManagerContext context) : BaseRepository<Order, long>(context), IOrderRepository
{
    protected override IQueryable<Order> IncludeAll(IQueryable<Order> queryable)
    {
        return queryable
            .Include(x => x.MenuItems);
    }

    public IAsyncEnumerable<Order> GetAsync(GetOrderFilter filter)
    {
        var query = IncludeAll(_context.Orders);

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
        _context.AttachRange(entity.MenuItems);
        _context.Orders.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return await IncludeAll(_context.Orders).SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);
    }
}