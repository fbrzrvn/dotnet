namespace FreeStuff.Contracts.Shared.Responses;

public class PagedResponse<TResponse>
{
    public IEnumerable<TResponse> Data         { get; init; } = Enumerable.Empty<TResponse>();
    public int                    Page         { get; init; }
    public int                    Limit        { get; init; }
    public int                    TotalResults { get; init; }
    public bool                   HasNextPage  => TotalResults > Page * Limit;
}
