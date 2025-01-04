

using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) :
 IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation($"[Start] Handling - request={typeof(TRequest).Name} - response={typeof(TResponse).Name}");
        var timer = new Stopwatch();
        timer.Start();
        var response = await next();
        timer.Stop();
        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning($"[Performance] Handling - request={typeof(TRequest).Name} - response={typeof(TResponse).Name} - TimeTaken={timeTaken}");
        }
        else
        {
            logger.LogInformation($"[End] Handling - request={typeof(TRequest).Name} - response={typeof(TResponse).Name} - TimeTaken={timeTaken}");
        }
        return response;
    }
}