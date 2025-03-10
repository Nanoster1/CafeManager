using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

/// <summary>
/// Контроллер для работы с миграциями
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class MigrationController : ControllerBase
{
    private readonly IMigrationService _migrationService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public MigrationController(IMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    /// <summary>
    /// Применить миграции
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("migrate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Migrate(CancellationToken cancellationToken)
    {
        await _migrationService.Migrate(cancellationToken);
        return NoContent();
    }
}