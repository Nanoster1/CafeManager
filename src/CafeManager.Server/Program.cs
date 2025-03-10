using CafeManager.Core;
using CafeManager.Data;
using CafeManager.Server.Middleware;

using Microsoft.OpenApi.Models;

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
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Swagger Cafe Manager",
            Description = "Swagger for Cafe Manager Server"
        });

        var xmlPath = $"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{builder.Environment.ApplicationName}.xml";
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath, true);
        }
    });
}

var app = builder.Build();
{
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();
}

app.Run();
