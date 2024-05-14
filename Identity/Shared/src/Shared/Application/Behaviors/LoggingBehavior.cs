namespace Shared.Application.Behaviors;

using MediatR;
using Serilog;
using IErrorOr = ErrorOr.IErrorOr;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : IErrorOr
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger.ForContext<LoggingBehavior<TRequest, TResponse>>();
    }

    public async Task<TResponse> Handle(
        TRequest                          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken                 cancellationToken
    )
    {
        _logger.Information("Starting request: {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

        var response = await next();

        if (response.IsError)
        {
            _logger.Error(
                "Request failure: {@RequestName}, {@Error}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                response.Errors,
                DateTime.UtcNow
            );
        }

        _logger.Information(
            "Completed request: {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );

        return response;
    }
}
