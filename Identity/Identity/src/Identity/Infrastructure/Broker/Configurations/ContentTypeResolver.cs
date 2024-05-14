namespace Identity.Infrastructure.Broker.Configurations;

using Domain.Events;
using KafkaFlow;
using KafkaFlow.Middlewares.Serializer.Resolvers;

public class ContentTypeResolver : IMessageTypeResolver
{
    private const string _messageType = "message_type";

    private readonly Dictionary<string, Type?> _messageTypeSelector = new()
    {
        { "chicly.accounts.event.identity.created", typeof(UserCreatedEvent) },
        { "chicly.accounts.event.identity.updated", typeof(UserCreatedEvent) },
        { "chicly.accounts.event.identity.deleted", typeof(UserCreatedEvent) }
    };

    public ValueTask<Type> OnConsumeAsync(IMessageContext context)
    {
        return ValueTask.FromResult(OnConsume(context))!;
    }

    public ValueTask OnProduceAsync(IMessageContext context)
    {
        OnProduce(context);

        return ValueTask.CompletedTask;
    }

    private Type? OnConsume(IMessageContext context)
    {
        var typeName = context.Headers.GetString(_messageType);

        return _messageTypeSelector[typeName];
    }

    private static void OnProduce(IMessageContext context)
    {
    }
}
