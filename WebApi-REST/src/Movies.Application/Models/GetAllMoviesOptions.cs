namespace Movies.Application.Models;

public class GetAllMoviesOptions
{
    public string? Title { get; set; }

    public int? Year { get; set; }

    public Guid? UserId { get; set; }

    public string? SortField { get; set; }

    public SortOrder? SortOrder { get; set; }

    public int Page { get; set; }

    public int Limit { get; set; }
}

public enum SortOrder
{
    UnSorted,
    Ascending,
    Descending,
}
