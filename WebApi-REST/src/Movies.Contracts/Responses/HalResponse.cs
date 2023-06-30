using System.Text.Json.Serialization;

namespace Movies.Contracts.Responses;

// HAL - Hypertext Application Language
public class HalResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Link> Links { get; set; } = new();
}

public class Link
{
    public required string Href { get; init; }

    public required string Ref { get; init; }

    public required string Type { get; init; }
}