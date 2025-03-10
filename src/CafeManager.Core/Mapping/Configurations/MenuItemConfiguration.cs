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
            .TwoWays();

        TypeAdapterConfig<AddMenuItemDto, MenuItem>
            .NewConfig();

        TypeAdapterConfig<UpdateMenuItemDto, MenuItem>
            .NewConfig();

        TypeAdapterConfig<long, MenuItem>
            .NewConfig()
            .MapWith(x => new MenuItem { Id = x, Name = string.Empty });
    }
}