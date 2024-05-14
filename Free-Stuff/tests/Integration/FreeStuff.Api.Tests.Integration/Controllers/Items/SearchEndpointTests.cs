using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FreeStuff.Contracts.Items.Responses;

namespace FreeStuff.Api.Tests.Integration.Controllers.Items;

public class SearchEndpointTests : IClassFixture<FreeStuffApiFactory>
{
    private readonly HttpClient _httpClient;

    public SearchEndpointTests(FreeStuffApiFactory freeStuffApiFactory)
    {
        _httpClient = freeStuffApiFactory.CreateClient();
    }

    [Theory]
    [InlineData("title=keyboard")]
    [InlineData("categoryName=games")]
    [InlineData("condition=as good")]
    [InlineData("title=keyboard&categoryName=games&condition=as good")]
    [InlineData("title=keyboard&categoryName=games&condition=new")]
    [InlineData("title=item&categoryName=games&condition=as good")]
    public async Task Search_ReturnsOkWithEmptyList_WhenNoItemsMatchQuery(string queryParams)
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Search}?{queryParams}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().BeEmpty();
    }

    [Theory]
    [InlineData("title=a")]
    [InlineData("condition=fair")]
    [InlineData("title=a&condition=new")]
    public async Task Search_ShouldReturnsOk_WhenItemsMatchQuery(string queryParams)
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Search}?{queryParams}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().ContainSingle();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Search_ShouldReturnOkWithItemsList_WhenItemsMatchQuery()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Search}?categoryName=Test",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().HaveCount(4);
    }

    [Fact]
    public async Task Search_ShouldReturnOkWithSortedItems_WhenSortByDesc()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Search}?title=item&sortBy=desc",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.First().Title.Should().Be("item D");
        actual?.Data.Last().Title.Should().Be("item A");
    }

    [Fact]
    public async Task Search_ShouldReturnOkWithSortedItems_WhenSortByAsc()
    {
        // Act
        var response = await _httpClient.GetAsync(
            $"{ApiEndpoints.Items.Search}?title=item&sortBy=asc",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.First().Title.Should().Be("item A");
        actual?.Data.Last().Title.Should().Be("item D");
    }

    [Fact]
    public async Task Search_ShouldReturnsOkWithItemsList_WhenCallWithoutQueryParams()
    {
        // Act
        var response = await _httpClient.GetAsync(
            ApiEndpoints.Items.Search,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var actual = await response.Content.ReadFromJsonAsync<ItemsResponse>();
        actual?.Data.Should().HaveCount(4);
    }
}
