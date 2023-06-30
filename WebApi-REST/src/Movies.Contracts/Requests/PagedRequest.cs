namespace Movies.Contracts.Requests;

public class PagedRequest
{
    public int Page { get; init; } = 1;

    public int Limit { get; init; } = 10;
}
