using System.Text.Json.Serialization;

namespace Movies.Api;

public class Movie
{
    [JsonPropertyName("pk")]
    public string Pk => ReleaseYear.ToString();

    [JsonPropertyName("sk")]
    public string Sk => Title.ToString();

    public Guid   Id                       { get; set; }
    public string Title                    { get; set; } = default!;
    public int    ReleaseYear              { get; set; }
    public int    AgeRestriction           { get; set; }
    public int    RottenTomatoesPercentage { get; set; }
}