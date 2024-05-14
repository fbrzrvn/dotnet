namespace Identity.Domain.Events;

using Microsoft.AspNetCore.Identity;

public abstract class EventData
{
    public string? Id       { get; private set; }
    public string? Username { get; private set; }
    public string? Email    { get; private set; }

    public EventData(IdentityUser user)
    {
        Id       = user.Id;
        Username = user.UserName;
        Email    = user.Email;
    }
}

public class UserCreatedEventData : EventData
{
    public UserCreatedEventData(IdentityUser user) : base(user)
    {
    }
}

public class UserUpdatedEventData : EventData
{
    public UserUpdatedEventData(IdentityUser user) : base(user)
    {
    }
}

public class UserDeletedEventData : EventData
{
    public UserDeletedEventData(IdentityUser user) : base(user)
    {
    }
}
