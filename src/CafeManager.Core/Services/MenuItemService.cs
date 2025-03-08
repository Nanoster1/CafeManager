
using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Common;
using CafeManager.Core.Exceptions;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services.Interfaces;

using MapsterMapper;

namespace CafeManager.Core.Services;

public class MenuItemService :
    CrudService<long, MenuItem, MenuItemDto, AddMenuItemDto, UpdateMenuItemDto>,
    IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper) : base(menuItemRepository, mapper)
    {
        _menuItemRepository = menuItemRepository;
    }

    public override async Task<MenuItemDto> CreateAsync(AddMenuItemDto dto, CancellationToken cancellationToken = default)
    {
        var itemWithSameNameExists = await _menuItemRepository.ExistsAsync(new MenuItemExistsFilter(dto.Name), cancellationToken);

        if (itemWithSameNameExists)
        {
            throw new EntityConflictException($"MenuItem with name {dto.Name} already exists.");
        }

        return await base.CreateAsync(dto, cancellationToken);
    }

    public override async Task<MenuItemDto> UpdateAsync(long id, UpdateMenuItemDto dto, CancellationToken cancellationToken = default)
    {
        var itemWithSameNameExists = await _menuItemRepository.ExistsAsync(new MenuItemExistsFilter(dto.Name), cancellationToken);

        if (itemWithSameNameExists)
        {
            throw new EntityConflictException($"MenuItem with name {dto.Name} already exists.");
        }

        return await base.UpdateAsync(id, dto, cancellationToken);
    }
}