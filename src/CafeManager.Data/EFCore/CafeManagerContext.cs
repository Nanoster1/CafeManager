using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Models.Orders;
using CafeManager.Data.EFCore.RelationModels;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore;

public class CafeManagerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderMenuItem> OrderMenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CafeManagerContext).Assembly);
    }
}