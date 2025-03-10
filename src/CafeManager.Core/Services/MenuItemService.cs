
using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Common;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services.Interfaces;

using MapsterMapper;

namespace CafeManager.Core.Services;

public class MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper) :
    CrudService<long, MenuItem, MenuItemDto, AddMenuItemDto, UpdateMenuItemDto>(menuItemRepository, mapper),
    IMenuItemService
{
}