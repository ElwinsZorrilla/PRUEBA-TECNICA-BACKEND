using System.Diagnostics;

namespace LibraryProxy.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int WarningThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > WarningThresholdMs)
        {
            Log.Warning("Long running request: {RequestName} ({ElapsedMs}ms) {@Request}",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, request);
        }

        return response;
    }
}
