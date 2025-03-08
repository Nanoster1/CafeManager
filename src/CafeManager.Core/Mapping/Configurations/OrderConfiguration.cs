using CafeManager.Contracts.Dto.Orders;
using CafeManager.Contracts.Dto.Orders.Enums;
using CafeManager.Core.Models.Orders;
using CafeManager.Core.Models.Orders.Enums;

using Mapster;

namespace CafeManager.Core.Mapping.Configurations;

public static class OrderConfiguration
{
    public static void Configure()
    {
        TypeAdapterConfig<OrderStatusDto, OrderStatus>
            .NewConfig()
            .TwoWays();

        TypeAdapterConfig<PaymentTypeDto, PaymentType>
            .NewConfig()
            .TwoWays();

        TypeAdapterConfig<Order, OrderDto>
            .NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<AddOrderDto, Order>
            .NewConfig()
            .MapToConstructor(true);

        TypeAdapterConfig<GetOrderFilterDto, GetOrderFilter>
            .NewConfig()
            .MapToConstructor(true);
    }
}