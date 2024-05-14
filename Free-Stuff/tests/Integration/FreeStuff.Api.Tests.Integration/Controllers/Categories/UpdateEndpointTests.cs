using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Contracts.Categories.Responses;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Categories;

public class UpdateEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public UpdateEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_ShouldUpdateCategory_WhenFoundAndValidRequestIsSent()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(
            Constants.Category.Test,
            Constants.Category.EditedName,
            Constants.Category.EditedDescription
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            ApiEndpoints.Category.Base,
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<CategoryResponse>();
        actual?.Name.Should().Be(Constants.Category.EditedName);
        actual?.Description.Should().Be(Constants.Category.EditedDescription);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenInvalidRequestIsSent()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(
            Constants.Category.Test,
            string.Empty,
            string.Empty
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            ApiEndpoints.Category.Base,
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ShouldReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var updateCategoryRequest = new UpdateCategoryRequest(
            Constants.Category.Name,
            Constants.Category.EditedName,
            Constants.Category.Description
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            ApiEndpoints.Category.Base,
            updateCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
