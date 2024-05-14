using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Categories;

public class CreateEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public CreateEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Create_ShouldCreateCategory_WhenValidRequestIsSent()
    {
        // Arrange
        var createCategoryRequest = new CreateCategoryRequest(
            Constants.Category.Name,
            Constants.Category.Description
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Category.Base,
            createCategoryRequest,
            CancellationToken.None
        );

        // Assert
        var actual = await response.Content.ReadFromJsonAsync<CategoryDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        actual.Should().NotBeNull().And.BeEquivalentTo(createCategoryRequest);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenInValidRequestIsSent()
    {
        // Arrange
        var createCategoryRequest = new CreateCategoryRequest(
            string.Empty,
            string.Empty
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Category.Base,
            createCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenCategoryNameAlreadyExist()
    {
        // Arrange
        var createCategoryRequest = new CreateCategoryRequest(
            Constants.Category.Test,
            Constants.Category.Description
        );

        // Act
        var response = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Category.Base,
            createCategoryRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
