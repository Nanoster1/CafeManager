using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Data.EFCore.Common.Classes;

namespace CafeManager.Data.EFCore.Repositories;

public class MenuItemRepository(CafeManagerContext context) : BaseRepository<MenuItem, long>(context), IMenuItemRepository
{
}