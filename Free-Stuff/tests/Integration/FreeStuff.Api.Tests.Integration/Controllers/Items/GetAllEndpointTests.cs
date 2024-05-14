using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Items.Responses;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class GetAllEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetAllEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnsOkWithItems_WhenItemsExist()
    {
        // Act
        var response = await _httpClient.GetAsync(
            ApiEndpoints.Items.Base,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().HaveCount(4);
        actual?.Page.Should().Be(1);
        actual?.Limit.Should().Be(10);
        actual?.TotalResults.Should().Be(4);
        actual?.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetAll_ShouldReturnsOkWithItems_WhenPageAndLimitAreSpecified()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Base}?page=2&limit=1",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().ContainSingle();
        actual?.Data.First().Title.Should().Be("item C");
        actual?.Page.Should().Be(2);
        actual?.Limit.Should().Be(1);
        actual?.TotalResults.Should().Be(4);
        actual?.HasNextPage.Should().BeTrue();
    }
}
