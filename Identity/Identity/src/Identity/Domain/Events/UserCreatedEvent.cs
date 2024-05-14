namespace Identity.Domain.Events;

using Shared.Domain;

public class UserCreatedEvent : Event
{
    public UserCreatedEvent()
    {
    }

    public UserCreatedEvent(UserCreatedEventData data)
    {
        Data = data;
        InitMeta();
    }

    public UserCreatedEventData? Data { get; set; }

    protected override string GetFullQualifiedEventName()
    {
        return "chicly.accounts.event.identity.created";
    }

    public override string GetMessageId()
    {
        return string.Empty;
    }
}
