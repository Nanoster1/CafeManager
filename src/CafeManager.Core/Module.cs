using CafeManager.Core.Mapping;
using CafeManager.Core.Services;
using CafeManager.Core.Services.Interfaces;

using Mapster;

using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.Core;

public static class Module
{
    public static IServiceCollection AddCoreModule(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IMenuItemService, MenuItemService>();
        services.AddMappingModule();
        return services;
    }
}