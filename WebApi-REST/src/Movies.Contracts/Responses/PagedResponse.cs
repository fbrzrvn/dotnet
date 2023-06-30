namespace Movies.Contracts.Responses;

public class PagedResponse<TResponse>
{
    public IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();

    public int? Page { get; init; }

    public int Limit { get; init; }

    public int Total { get; init; }

    public bool HasNextPage => Total > (Page * Limit);
}
