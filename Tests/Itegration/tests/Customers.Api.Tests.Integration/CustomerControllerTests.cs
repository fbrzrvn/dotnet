using System.Net;

namespace Customers.Api.Tests.Integration;

public class CustomerControllerTests : IAsyncLifetime
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:5001")
    };

    public CustomerControllerTests()
    {
        // Sync setup code here
    }

    public async Task InitializeAsync()
    {
        // Async setup code
        await Task.Delay(1000);
    }

    public Task DisposeAsync()
    {
        // Clean up code here
        return Task.CompletedTask;
    }


    [Fact]
    public async Task Get_ShouldReturnsNotFound_WhenCustomerDoesNotExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"customers/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
