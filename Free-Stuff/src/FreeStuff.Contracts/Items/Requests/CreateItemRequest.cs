namespace FreeStuff.Contracts.Items.Requests;

public record CreateItemRequest
(
    string Title,
    string Description,
    string CategoryName,
    string Condition,
    Guid   UserId
);
