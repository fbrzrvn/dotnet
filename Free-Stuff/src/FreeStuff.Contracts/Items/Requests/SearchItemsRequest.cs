using FreeStuff.Contracts.Shared.Requests;

namespace FreeStuff.Contracts.Items.Requests;

public class SearchItemsRequest : PagedRequest
{
    public string? Title        { get; init; } = string.Empty;
    public string? CategoryName { get; init; } = string.Empty;
    public string? Condition    { get; init; } = string.Empty;
    public string? SortBy       { get; init; } = string.Empty;
}
