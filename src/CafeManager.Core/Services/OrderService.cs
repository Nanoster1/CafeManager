

using CafeManager.Contracts.Dto.Orders;
using CafeManager.Core.Exceptions;
using CafeManager.Core.Models.Orders;
using CafeManager.Core.Models.Orders.Enums;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services.Interfaces;

using MapsterMapper;

namespace CafeManager.Core.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task CompleteOrderAsync(long orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _orderRepository.GetAsync(orderId, cancellationToken) ??
            throw new EntityNotFoundException($"Order with id {orderId} not found.");

        entity.Status = OrderStatus.Completed;

        await _orderRepository.UpdateAsync(entity, cancellationToken);
    }

    public async Task<OrderDto> CreateAsync(AddOrderDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Order>(dto);
        entity.Status = OrderStatus.InWork;

        var result = await _orderRepository.CreateAsync(entity, cancellationToken);

        return _mapper.Map<OrderDto>(result);
    }

    public IAsyncEnumerable<OrderDto> GetAsync(GetOrderFilterDto filterDto)
    {
        var filter = _mapper.Map<GetOrderFilter>(filterDto);
        var entities = _orderRepository.GetAsync(filter);
        return entities.Select(_mapper.Map<OrderDto>);
    }
}