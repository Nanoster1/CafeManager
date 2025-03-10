using CafeManager.Contracts.Dto.Orders.Enums;

namespace CafeManager.Contracts.Dto.Orders;

public record PartialUpdateOrderDto(
    string? CustomerName,
    DateTimeOffset? CompletedAt,
    PaymentTypeDto? PaymentType,
    List<long>? MenuItemIds
);