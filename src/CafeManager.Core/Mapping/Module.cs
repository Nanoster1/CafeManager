using CafeManager.Core.Mapping.Configurations;

using Mapster;

using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.Core.Mapping;

public static class Module
{
    public static IServiceCollection AddMappingModule(this IServiceCollection services)
    {
        OrderConfiguration.Configure();
        MenuItemConfiguration.Configure();
        services.AddMapster();
        return services;
    }
}