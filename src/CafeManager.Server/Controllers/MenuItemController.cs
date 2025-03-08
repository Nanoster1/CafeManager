using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;

    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> GetAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.GetAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> CreateAsync([FromBody] AddMenuItemDto dto, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.CreateAsync(dto, cancellationToken);
        return Created(string.Empty, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType<MenuItemDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MenuItemDto>> UpdateAsync([FromRoute] long id, [FromBody] UpdateMenuItemDto dto, CancellationToken cancellationToken)
    {
        var result = await _menuItemService.UpdateAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
    {
        await _menuItemService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}