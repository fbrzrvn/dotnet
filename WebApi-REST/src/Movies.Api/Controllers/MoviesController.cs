using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mappers;
using Movies.Application.Services;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Movies.Create)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
    {
        var movie = request.MapToMovie();

        await _movieService.CreateAsync(movie, token);

        //return Created($"/{ApiEndpoints.Movies.Create}/{movie.Id}", movie);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    [ProducesResponseType(typeof(MoviesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var options = request.MapToOptions().WithUser(userId);

        var movies = await _movieService.GetAllAsync(options, token);
        var moviesCount = await _movieService.GetCountAsync(options.Title, options.Year, token);

        return Ok(movies.MapToResponse(request.Page, request.Limit, moviesCount));
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, userId, token)
            : await _movieService.GetBySlugAsync(idOrSlug, userId, token);

        return movie is null ? NotFound() : Ok(movie.MapToResponse());
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Movies.Update)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var movie = request.MapToMovie(id);

        var updatedMovie = await _movieService.UpdateAsync(movie, userId, token);

        return updatedMovie is null ? NotFound() : Ok(movie.MapToResponse());
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Movies.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _movieService.DeleteByIdAsync(id, token);

        return !deleted ? NotFound() : Ok();
    }
}
