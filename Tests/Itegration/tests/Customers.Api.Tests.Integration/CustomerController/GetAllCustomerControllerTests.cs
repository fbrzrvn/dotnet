using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.Tests.Integration.CustomerController;

// [Collection("CustomerApi Collection")]
public class GetAllCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private readonly HttpClient _httpClient;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ValidGithubUser)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public GetAllCustomerControllerTests(CustomerApiFactory appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ShouldReturnsAllCustomers_WhenCustomersExist()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _httpClient.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        // Act
        var response = await _httpClient.GetAsync("customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customersResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        customersResponse!.Customers.Single().Should().BeEquivalentTo(createdCustomer);

        // Cleanup
        await _httpClient.DeleteAsync($"customers/{createdCustomer!.Id}");
    }

    [Fact]
    public async Task GetAll_ShouldReturnsEmptyResult_WhenNoCustomersExist()
    {
        // Act
        var response = await _httpClient.GetAsync($"customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customersResponse = await response.Content.ReadFromJsonAsync<GetAllCustomersResponse>();
        customersResponse!.Customers.Should().BeEmpty();
    }
}
