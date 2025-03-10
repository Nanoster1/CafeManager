using CafeManager.Contracts.Dto.Orders;
using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

/// <summary>
/// Контроллер для работы с заказами
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Получение заказов
    /// </summary>
    /// <param name="filterDto">Фильтр</param>
    /// <returns>Список заказов</returns>
    [HttpGet]
    [ProducesResponseType<IAsyncEnumerable<OrderDto>>(StatusCodes.Status200OK)]
    public ActionResult<IAsyncEnumerable<OrderDto>> GetAsync([FromQuery] GetOrderFilterDto filterDto)
    {
        var result = _orderService.GetAsync(filterDto);
        return Ok(result);
    }

    /// <summary>
    /// Создание заказа
    /// </summary>
    /// <param name="dto">Данные для создания</param>
    /// <returns>Созданный заказ</returns>
    [HttpPost]
    [ProducesResponseType<OrderDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] AddOrderDto dto)
    {
        var result = await _orderService.CreateAsync(dto);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Завершение заказа
    /// </summary>
    /// <param name="id">Id заказа</param>
    [HttpPost("{id}:complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CompleteOrderAsync([FromRoute] long id)
    {
        await _orderService.CompleteOrderAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Отмена заказа
    /// </summary>
    /// <param name="id">Id заказа</param>
    [HttpPost("{id}:cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CancelOrderAsync([FromRoute] long id)
    {
        await _orderService.CancelOrderAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Частичное обновление заказа
    /// </summary>
    /// <param name="id">Id заказа</param>
    /// <param name="dto">Данные для обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Обновленный заказ</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> PartialUpdateAsync([FromRoute] long id, [FromBody] PartialUpdateOrderDto dto, CancellationToken cancellationToken)
    {
        var result = await _orderService.PartialUpdateAsync(id, dto, cancellationToken);
        return Ok(result);
    }
}