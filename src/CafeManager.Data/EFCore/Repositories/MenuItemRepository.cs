
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common;

using Microsoft.Extensions.Logging;

namespace CafeManager.Data.EFCore.Repositories;

public class MenuItemRepository(
    CafeManagerContext context,
    ILogger<MenuItemRepository> logger) :
    BaseRepository<MenuItem, long>(context, logger),
    IMenuItemRepository
{
}