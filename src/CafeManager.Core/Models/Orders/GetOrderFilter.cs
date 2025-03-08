using CafeManager.Core.Models.Orders.Enums;

namespace CafeManager.Core.Models.Orders;

public record GetOrderFilter(DateTimeOffset? From, DateTimeOffset? To, OrderStatus? Status);