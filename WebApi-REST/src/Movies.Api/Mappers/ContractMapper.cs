using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappers;

public static class ContractMapper
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
        };
    }

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies, int page, int limit, int totalCount)
    {
        return new MoviesResponse
        {
            Items = movies.Select(MapToResponse),
            Page = page,
            Limit = limit,
            Total = totalCount
        };
    }

    public static MovieResponse MapToResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Slug = movie.Slug,
            Genres = movie.Genres,
            Rating = movie.Rating,
            UserRating = movie.UserRating,
        };
    }

    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
        };
    }

    public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
    {
        return ratings.Select(x => new MovieRatingResponse
        {
            MovieId = x.MovieId,
            Slug = x.Slug,
            Rating = x.Rating,
        });
    }

    public static GetAllMoviesOptions MapToOptions(this GetAllMoviesRequest request)
    {
        return new GetAllMoviesOptions
        {
            Title = request.Title,
            Year = request.Year,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? SortOrder.UnSorted : request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
            Page = request.Page,
            Limit = request.Limit,
        };
    }

    public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;

        return options;
    }
}
