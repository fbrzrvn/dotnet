namespace Shared.Infrastructure.Broker;

using Domain;

public interface IMessageBroker
{
    Task<string> PublishAsync(Event @event);
    Task<string> PublishAsync(Event @event, string topicName);
}
