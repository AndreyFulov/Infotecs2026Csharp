using Microsoft.AspNetCore.Diagnostics;

namespace InfotecsTestApplication.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception.Message);
        var statusCode = httpContext.Response.StatusCode;
        return ValueTask.FromResult(statusCode >= 400);
    }
}