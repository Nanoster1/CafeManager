using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Models.MenuItems;

namespace CafeManager.Core.Repositories;

public interface IMenuItemRepository : IBaseRepository<MenuItem, long>
{
    Task<bool> ExistsAsync(MenuItemExistsFilter filter, CancellationToken cancellationToken = default);
}