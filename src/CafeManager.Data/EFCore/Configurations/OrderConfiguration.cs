using CafeManager.Core.Models.Orders;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafeManager.Data.EFCore.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.CustomerName).IsRequired();
        builder.Property(x => x.CompletedAt).IsRequired();
        builder.Property(x => x.PaymentType).IsRequired();

        builder.HasMany(x => x.MenuItems)
            .WithMany();
    }
}