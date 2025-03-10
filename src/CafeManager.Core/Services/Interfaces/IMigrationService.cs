namespace CafeManager.Core.Services.Interfaces;

public interface IMigrationService
{
    Task Migrate(CancellationToken cancellationToken);
}