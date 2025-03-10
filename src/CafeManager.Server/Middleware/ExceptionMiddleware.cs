using CafeManager.Core.Exceptions;
using CafeManager.Server.Constants;

namespace CafeManager.Server.Middleware;

/// <summary>
/// Middleware для обработки исключений
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Middleware Invoke
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            await WriteErrorToResponseAsync(context.Response, e, $"{context.Request.Method}: {context.Request.Path}", context.RequestAborted);
        }
    }

    private async Task WriteErrorToResponseAsync(HttpResponse response, Exception e, string actionName, CancellationToken ct)
    {
        response.StatusCode = e switch
        {
            InvalidInputDataException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            EntityConflictException => StatusCodes.Status409Conflict,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = e switch
        {
            EntityNotFoundException => e.Message,
            EntityConflictException => e.Message,
            _ => "Something went wrong"
        };

        response.ContentType = MimeTypes.PlainText;
        await response.WriteAsync(message, ct);

        if (response.StatusCode is StatusCodes.Status500InternalServerError)
        {
            _logger.LogError("{actionName}: {e}.", actionName, e);
        }
        else
        {
            _logger.LogWarning("{actionName}: {e}.", actionName, e);
        }
    }
}
