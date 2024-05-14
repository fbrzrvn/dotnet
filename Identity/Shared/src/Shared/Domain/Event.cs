namespace Shared.Domain;

using Newtonsoft.Json;

public abstract class Event
{
    private readonly DateTime _occurredOn = DateTime.UtcNow;

    [JsonProperty(Order = 1)]
    public EventMeta? Meta { get; set; }

    public abstract string GetMessageId();

    protected void InitMeta(EventMeta metaEventSource)
    {
        var transactionId = metaEventSource.TransactionId;
        Meta = EventMeta.Create(GetFullQualifiedEventName(), GetOccurredOn(), transactionId);
    }

    protected void InitMeta()
    {
        Meta = EventMeta.Create(GetFullQualifiedEventName(), GetOccurredOn(), null);
    }

    protected abstract string GetFullQualifiedEventName();

    private DateTime GetOccurredOn()
    {
        return _occurredOn;
    }
}
