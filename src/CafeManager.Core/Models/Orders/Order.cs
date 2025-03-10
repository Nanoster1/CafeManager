using System.ComponentModel.DataAnnotations;

using CafeManager.Core.Common.Interfaces;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Models.Orders.Enums;

namespace CafeManager.Core.Models.Orders;

public class Order : IEntity<long>
{
    public required long Id { get; init; }

    [MinLength(1)]
    public required string CustomerName { get; set; }

    public required DateTimeOffset CompletedAt { get; set; }

    public required PaymentType PaymentType { get; set; }

    public required OrderStatus Status { get; set; }

    public List<MenuItem> MenuItems { get; set; } = [];
}