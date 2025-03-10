using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

/// <summary>
/// Контроллер для работы с меню
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    /// <summary>
    /// Получение позиции меню
    /// </summary>
    /// <param name="id">Id позиции меню</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Позиция меню</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> GetAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.GetAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Создание позиции меню
    /// </summary>
    /// <param name="dto">Данные для создания</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Созданная позиция меню</returns>
    [HttpPost]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> CreateAsync([FromBody] AddMenuItemDto dto, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.CreateAsync(dto, cancellationToken);
        return Created(string.Empty, result);
    }

    /// <summary>
    /// Обновление позиции меню
    /// </summary>
    /// <param name="id">Id позиции меню</param>
    /// <param name="dto">Данные для обновления</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Обновленная позиция меню</returns>
    [HttpPut("{id}")]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> UpdateAsync([FromRoute] long id, [FromBody] UpdateMenuItemDto dto, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.UpdateAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Удаление позиции меню
    /// </summary>
    /// <param name="id">Id позиции меню</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Удаленная позиция меню</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        await _menuItemService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}