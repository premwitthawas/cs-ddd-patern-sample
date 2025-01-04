
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Shared.Exceptions.Handlers;

public class CustomExceptionHandle(ILogger<CustomExceptionHandle> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError($"An exception occurred: {exception.Message} - time of occurred {DateTime.UtcNow}");
        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            ),
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            BadRequestException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            NotFoundException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status404NotFound
            ),
            _ => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };

        var problemsDetails = new ProblemDetails{
            Detail = details.Detail,
            Title = details.Title,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };

        problemsDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);
        if(exception is ValidationException validationException)
        {
            problemsDetails.Extensions.Add("errors", validationException.Errors);
        }
        httpContext.Response.StatusCode = details.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(problemsDetails, cancellationToken);
        return true;    
    }
}