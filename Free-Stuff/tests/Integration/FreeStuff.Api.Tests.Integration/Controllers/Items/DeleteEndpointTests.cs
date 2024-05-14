using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class DeleteEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public DeleteEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Delete_ShouldReturnsNoContent_WhenItemExists()
    {
        // Arrange
        var createItemRequest = new CreateItemRequest(
            Constants.Item.Title,
            Constants.Item.Description,
            Constants.Category.Test,
            Constants.Item.Condition,
            Constants.Item.UserId
        );
        var createdResponse = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Items.Base,
            createItemRequest,
            CancellationToken.None
        );
        var item = await createdResponse.Content.ReadFromJsonAsync<ItemDto>();

        // Act
        var response = await _httpClient.DeleteAsync($"{ApiEndpoints.Items.Base}/{item!.Id}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ShouldReturnsNotFound_WhenItemDoesNotExist()
    {
        // Act
        var response = await _httpClient.DeleteAsync(
            $"{ApiEndpoints.Items.Base}/{Guid.NewGuid()}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
