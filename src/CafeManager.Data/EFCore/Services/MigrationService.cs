using CafeManager.Core.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace CafeManager.Data.EFCore.Services;

public class MigrationService : IMigrationService
{
    private readonly CafeManagerContext _context;

    public MigrationService(CafeManagerContext context)
    {
        _context = context;
    }

    public async Task Migrate(CancellationToken cancellationToken)
    {
        await _context.Database.MigrateAsync(cancellationToken);
    }
}