namespace Shared.Domain;

public class EventMeta
{
    public EventType? MessageType   { get; set; }
    public DateTime   OccurredOn    { get; set; }
    public string?    TransactionId { get; set; } = string.Empty;

    private EventMeta()
    {
    }

    private EventMeta(string messageType, DateTime occurredOn, string? transactionId)
    {
        MessageType   = new EventType(messageType);
        OccurredOn    = occurredOn;
        TransactionId = transactionId;
    }

    public static EventMeta Create(string messageType, DateTime occurredOn, string? transactionId)
    {
        return new EventMeta(messageType, occurredOn, transactionId);
    }
}
