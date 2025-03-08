using System.ComponentModel.DataAnnotations;

using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Contracts.Dto.Orders.Enums;

namespace CafeManager.Contracts.Dto.Orders;

public record OrderDto(
    [Required] long Id,
    [Required] string CustomerName,
    [Required] DateTimeOffset CompletedAt,
    [Required] PaymentTypeDto PaymentType,
    [Required] OrderStatusDto Status,
    [Required] List<MenuItemDto> MenuItems
);