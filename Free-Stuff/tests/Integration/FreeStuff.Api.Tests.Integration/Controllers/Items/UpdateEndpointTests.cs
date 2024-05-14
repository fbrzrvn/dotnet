using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Contracts.Items.Requests;
using FreeStuff.Items.Application.Shared.Dto;
using FreeStuff.Tests.Utils.Constants;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class UpdateEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly CreateItemRequest _createItemRequest = new(
        Constants.Item.Title,
        Constants.Item.Description,
        Constants.Category.Test,
        Constants.Item.Condition,
        Constants.Item.UserId
    );

    public UpdateEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task Update_ShouldUpdateItem_WhenFoundAndValidRequestIsSent()
    {
        // Arrange
        var createdResponse = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Items.Base,
            _createItemRequest,
            CancellationToken.None
        );

        await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Category.Base,
            new CreateCategoryRequest(
                Constants.Category.EditedName,
                Constants.Category.Description
            ),
            CancellationToken.None
        );

        var item = await createdResponse.Content.ReadFromJsonAsync<ItemDto>();

        var updateItemRequest = new UpdateItemRequest(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Category.EditedName,
            Constants.Item.EditedCondition,
            Constants.Item.UserId
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiEndpoints.Items.Base}/{item!.Id}",
            updateItemRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenInvalidRequestIsSent()
    {
        // Arrange
        var createdResponse = await _httpClient.PostAsJsonAsync(
            ApiEndpoints.Items.Base,
            _createItemRequest,
            CancellationToken.None
        );
        var item = await createdResponse.Content.ReadFromJsonAsync<ItemDto>();

        var updateItemRequest = new UpdateItemRequest(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Category.EditedName,
            "Old but gold",
            Constants.Item.UserId
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiEndpoints.Items.Base}/{item!.Id}",
            updateItemRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ShouldReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var updateItemRequest = new UpdateItemRequest(
            Constants.Item.EditedTitle,
            Constants.Item.EditedDescription,
            Constants.Category.EditedName,
            Constants.Item.EditedCondition,
            Constants.Item.UserId
        );

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            $"{ApiEndpoints.Items.Base}/{Guid.NewGuid()}",
            updateItemRequest,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
