namespace Shared.Infrastructure.Broker.Configurations;

public sealed class KafkaClientConfigurations
{
    public const string Section = "Kafka";

    public List<string> BootstrapServers { get; init; } = default!;
    public string       GroupId          { get; init; } = string.Empty;
    public string       TopicName        { get; init; } = string.Empty;
    public string       ProducerName     { get; init; } = string.Empty;
}
