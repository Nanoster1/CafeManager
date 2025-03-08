using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Common.Interfaces;

namespace CafeManager.Core.Services.Interfaces;

public interface IMenuItemService : ICrudService<long, MenuItemDto, AddMenuItemDto, UpdateMenuItemDto>
{

}