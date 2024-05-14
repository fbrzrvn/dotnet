using FreeStuff.Contracts.Items.Events;
using MassTransit;

namespace FreeStuff.Consumer.Consumer.Item;

public class ItemDeletedConsumer : IConsumer<ItemDeleted>
{
    private readonly ILogger<ItemDeletedConsumer> _logger;

    public ItemDeletedConsumer(ILogger<ItemDeletedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ItemDeleted> context)
    {
        _logger.LogInformation(
            "Timestamp --> {timestamp} | MessageId --> {messageId} | ItemDeleted --> {item}",
            context.SentTime,
            context.MessageId,
            context.Message
        );
        return Task.CompletedTask;
    }
}
