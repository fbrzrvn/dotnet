using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Categories.Responses;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Categories;

public class GetAllEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithCategories_WhenCategoriesExist()
    {
        // Act
        var response = await _httpClient.GetAsync(
            ApiEndpoints.Category.Base,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();
        actual.Should().HaveCount(1);
        actual?.First().Name.Should().Be(Constants.Category.Test);
    }
}
