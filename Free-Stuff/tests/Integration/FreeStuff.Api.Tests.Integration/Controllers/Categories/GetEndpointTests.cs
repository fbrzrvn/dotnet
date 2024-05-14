using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Categories.Responses;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Categories;

public class GetEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldReturnsOk_WhenCategoryExists()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Category.Base}/{Constants.Category.Test}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<CategoryResponse>();
        actual?.Id.Should().NotBeEmpty();
        actual?.Name.Should().Be(Constants.Category.Test);
        actual?.Description.Should().Be(Constants.Category.Description);
    }

    [Fact]
    public async Task Get_ShouldReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Category.Base}/{Constants.Category.Name}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
