using CafeManager.Core;
using CafeManager.Data;
using CafeManager.Server.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
{
    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    configuration.AddEnvironmentVariables();
}

var services = builder.Services;
{
    services.AddCoreModule();
    services.AddDataModule(builder.Configuration);

    services.AddControllers();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

var app = builder.Build();
{
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();
}

app.Run();
