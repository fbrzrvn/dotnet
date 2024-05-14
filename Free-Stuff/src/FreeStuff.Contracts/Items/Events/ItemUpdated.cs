namespace FreeStuff.Contracts.Items.Events;

public record ItemUpdated
(
    Guid     Id,
    string   Title,
    string   Description,
    string   Category,
    string   Condition,
    Guid     UserId,
    DateTime UpdatedDateTime
);
