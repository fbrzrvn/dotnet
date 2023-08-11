using System.Net;

namespace Movies.Api.Tests.Integration;

public class MoviesControllerTests
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5211/api")
    };


    [Fact]
    public async Task GetAll_ShouldReturnsOK_WhenMoviesExists()
    {
        // Act
        var response = await _httpClient.GetAsync("/movies");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
