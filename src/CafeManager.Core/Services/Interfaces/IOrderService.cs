using CafeManager.Contracts.Dto.Orders;

namespace CafeManager.Core.Services.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(AddOrderDto dto, CancellationToken cancellationToken = default);
    Task CompleteOrderAsync(long orderId, CancellationToken cancellationToken = default);
    Task CancelOrderAsync(long id, CancellationToken cancellationToken = default);
    IAsyncEnumerable<OrderDto> GetAsync(GetOrderFilterDto filterDto);
    Task<OrderDto> PartialUpdateAsync(long id, PartialUpdateOrderDto dto, CancellationToken cancellationToken = default);
}