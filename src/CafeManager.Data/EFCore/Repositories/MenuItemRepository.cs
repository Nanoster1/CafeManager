
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore.Repositories;

public class MenuItemRepository(CafeManagerContext context) : BaseRepository<MenuItem, long>(context), IMenuItemRepository
{
    public async Task<bool> ExistsAsync(MenuItemExistsFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.MenuItems.AsQueryable();

        query = filter.Name is not null
            ? query.Where(x => x.Name == filter.Name)
            : query;

        return await query.AnyAsync(cancellationToken);
    }
}