using CafeManager.Contracts.Dto.Orders.Enums;

namespace CafeManager.Contracts.Dto.Orders;

public record GetOrderFilterDto(
    DateTime? From = null,
    DateTime? To = null,
    OrderStatusDto? Status = null
);