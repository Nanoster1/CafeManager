using CafeManager.Core.Repositories;
using CafeManager.Core.Services.Interfaces;
using CafeManager.Data.Constants;
using CafeManager.Data.EFCore;
using CafeManager.Data.EFCore.Repositories;
using CafeManager.Data.EFCore.Services;

using EntityFramework.Exceptions.PostgreSQL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.Data;

public static class Module
{
    public static IServiceCollection AddDataModule(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseConnectionString = configuration.GetConnectionString(ConfigurationKeys.DatabaseConnectionString);

        services.AddDbContext<CafeManagerContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString);
            options.UseSnakeCaseNamingConvention();
            options.UseAllCheckConstraints();
            options.UseExceptionProcessor();
        });

        services.AddScoped<IMigrationService, MigrationService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        return services;
    }
}