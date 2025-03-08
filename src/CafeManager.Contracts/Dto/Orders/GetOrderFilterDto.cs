using CafeManager.Contracts.Dto.Orders.Enums;

namespace CafeManager.Contracts.Dto.Orders;

public record GetOrderFilterDto(DateTime? From, DateTime? To, OrderStatusDto? Status);