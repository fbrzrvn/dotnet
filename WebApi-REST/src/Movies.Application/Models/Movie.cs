﻿using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public partial class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public required int YearOfRelease { get; set; }

    public string Slug => GenerateSlug();

    public required List<string> Genres { get; init; } = new();

    public float? Rating { get; set; }

    public int? UserRating { get; set; }

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");

        return $"{sluggedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z ]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}
