using CafeManager.Core.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace CafeManager.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MigrationController : ControllerBase
{
    private readonly IMigrationService _migrationService;

    public MigrationController(IMigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    [HttpPost("migrate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Migrate(CancellationToken cancellationToken)
    {
        await _migrationService.Migrate(cancellationToken);
        return NoContent();
    }
}