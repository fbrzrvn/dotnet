using FreeStuff.Contracts.Items.Events;
using MassTransit;

namespace FreeStuff.Consumer.Consumer.Item;

public sealed class ItemCreatedConsumer : IConsumer<ItemCreated>
{
    private readonly ILogger<ItemCreatedConsumer> _logger;

    public ItemCreatedConsumer(ILogger<ItemCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ItemCreated> context)
    {
        _logger.LogInformation(
            "Timestamp --> {timestamp} | MessageId --> {messageId} | ItemCreated --> {item}",
            context.SentTime,
            context.MessageId,
            context.Message
        );

        return Task.CompletedTask;
    }
}
