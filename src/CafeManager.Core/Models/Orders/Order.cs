using System.ComponentModel.DataAnnotations;

using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Models.Orders.Enums;

namespace CafeManager.Core.Models.Orders;

public class Order(
    string customerName,
    DateTimeOffset completedAt,
    PaymentType paymentType,
    OrderStatus status,
    List<MenuItem>? menuItems = null) : IEntity<long>
{
    [Required]
    public long Id { get; }

    [Required, MinLength(1)]
    public string CustomerName { get; set; } = customerName;

    [Required, MinLength(1)]
    public DateTimeOffset CompletedAt { get; set; } = completedAt;

    [Required]
    public PaymentType PaymentType { get; set; } = paymentType;

    [Required]
    public OrderStatus Status { get; set; } = status;

    public List<MenuItem>? MenuItems { get; set; } = menuItems;
}