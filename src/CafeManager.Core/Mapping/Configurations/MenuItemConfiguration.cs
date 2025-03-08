using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Models.MenuItems;

using Mapster;

namespace CafeManager.Core.Mapping.Configurations;

public static class MenuItemConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<MenuItem, MenuItemDto>
            .NewConfig()
            .MapToConstructor(true)
            .TwoWays();

        TypeAdapterConfig<AddMenuItemDto, MenuItem>
            .NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<UpdateMenuItemDto, MenuItem>
            .NewConfig()
            .MapToConstructor(true);
    }
}