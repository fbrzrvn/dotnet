using FreeStuff.Shared.Domain;
using MassTransit;
using Serilog;

namespace FreeStuff.Shared.Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
    private readonly IBus    _bus;
    private readonly ILogger _logger;

    public EventBus(IBus bus, ILogger logger)
    {
        _bus    = bus;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            await _bus.Publish(message, cancellationToken);
            _logger.Information(
                "Successfully published message of type {@MessageType}, {@DateTimeUtc}.",
                typeof(T).FullName,
                DateTime.UtcNow
            );
        }
        catch (Exception ex)
        {
            _logger.Error(
                "Error publishing message of type {@MessageType}, {@Error}, {@DateTimeUtc}.",
                typeof(T).FullName,
                ex.Message,
                DateTime.UtcNow
            );
            throw;
        }
    }
}
