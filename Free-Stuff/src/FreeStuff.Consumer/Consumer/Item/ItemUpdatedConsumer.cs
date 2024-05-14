using FreeStuff.Contracts.Items.Events;
using MassTransit;

namespace FreeStuff.Consumer.Consumer.Item;

public class ItemUpdatedConsumer : IConsumer<ItemUpdated>
{
    private readonly ILogger<ItemUpdatedConsumer> _logger;

    public ItemUpdatedConsumer(ILogger<ItemUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ItemUpdated> context)
    {
        _logger.LogInformation(
            "Timestamp --> {timestamp} | MessageId --> {messageId} | ItemUpdated --> {item}",
            context.SentTime,
            context.MessageId,
            context.Message
        );
        return Task.CompletedTask;
    }
}
