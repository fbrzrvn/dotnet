namespace Shared.Infrastructure.Broker;

using Configurations;
using Domain;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.Extensions.Options;

public class KafkaMessageBroker : IMessageBroker
{
    private const string _messageType        = "message_type";
    private const string _messageTypeVersion = "message_type_version";

    private readonly KafkaClientConfigurations _configuration;
    private readonly IProducerAccessor         _producers;

    public KafkaMessageBroker(IProducerAccessor producers, IOptions<KafkaClientConfigurations> configuration)
    {
        _producers     = producers;
        _configuration = configuration.Value;
    }

    public async Task<string> PublishAsync(Event @event)
    {
        IMessageHeaders headers = new MessageHeaders();
        headers.SetString(_messageType, @event.Meta!.MessageType!.Name);
        headers.SetString(_messageTypeVersion, @event.Meta!.MessageType!.Version);

        var result = await _producers[_configuration.ProducerName]
            .ProduceAsync(
                _configuration.TopicName,
                @event.GetMessageId(),
                @event,
                headers
            );

        return result.TopicPartitionOffset.ToString();
    }

    public async Task<string> PublishAsync(Event @event, string topicName)
    {
        IMessageHeaders headers = new MessageHeaders();
        headers.SetString(_messageType, @event.Meta!.MessageType!.Name);
        headers.SetString(_messageTypeVersion, @event.Meta!.MessageType!.Version);

        var result = await _producers[_configuration.ProducerName]
            .ProduceAsync(
                topicName,
                @event.GetMessageId(),
                @event,
                headers
            );

        return result.TopicPartitionOffset.ToString();
    }
}
