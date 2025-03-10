using System.ComponentModel.DataAnnotations;

using CafeManager.Contracts.Dto.Orders.Enums;

namespace CafeManager.Contracts.Dto.Orders;

public record AddOrderDto(
    [Required] string CustomerName,
    [Required] DateTimeOffset CompletedAt,
    [Required] PaymentTypeDto PaymentType,
    [Required] List<long> MenuItemIds
);