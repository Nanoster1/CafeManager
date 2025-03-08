using CafeManager.Contracts.Dto.Orders;
using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType<IAsyncEnumerable<OrderDto>>(StatusCodes.Status200OK)]
    public ActionResult<IAsyncEnumerable<OrderDto>> GetAsync([FromQuery] GetOrderFilterDto filterDto)
    {
        var result = _orderService.GetAsync(filterDto);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<OrderDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] AddOrderDto dto)
    {
        var result = await _orderService.CreateAsync(dto);
        return Created(string.Empty, result);
    }

    [HttpPost("{id}:complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CompleteOrderAsync([FromRoute] long id)
    {
        await _orderService.CompleteOrderAsync(id);
        return NoContent();
    }
}