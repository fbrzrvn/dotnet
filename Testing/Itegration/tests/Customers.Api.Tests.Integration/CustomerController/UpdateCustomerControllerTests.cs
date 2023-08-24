using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.Tests.Integration.CustomerController;

public class UpdateCustomerControllerTests : IClassFixture<CustomerApiFactory>
{
    private const string invalidData = "dasdja9d3j";

    private readonly HttpClient _httpClient;

    private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
        .RuleFor(x => x.Email, faker => faker.Person.Email)
        .RuleFor(x => x.FullName, faker => faker.Person.FullName)
        .RuleFor(x => x.GitHubUsername, CustomerApiFactory.ValidGithubUser)
        .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date);

    public UpdateCustomerControllerTests(CustomerApiFactory appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }

    [Fact]
    public async Task Update_ShouldUpdatesUser_WhenDataIsValid()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _httpClient.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        customer = _customerGenerator.Generate();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"customers/{createdCustomer!.Id}", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
        customerResponse.Should().BeEquivalentTo(customer);
    }

    [Fact]
    public async Task Update_ShouldReturnsValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _httpClient.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        customer = _customerGenerator.Clone()
            .RuleFor(x => x.Email, invalidData).Generate();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"customers/{createdCustomer!.Id}", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["Email"][0].Should().Be($"{invalidData} is not a valid email address");
    }

    [Fact]
    public async Task Update_ReturnsValidationError_WhenGitHubUserDoestNotExist()
    {
        // Arrange
        var customer = _customerGenerator.Generate();
        var createdResponse = await _httpClient.PostAsJsonAsync("customers", customer);
        var createdCustomer = await createdResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        customer = _customerGenerator.Clone()
            .RuleFor(x => x.GitHubUsername, invalidData).Generate();

        // Act
        var response = await _httpClient.PutAsJsonAsync($"customers/{createdCustomer!.Id}", customer);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        error!.Status.Should().Be(400);
        error.Title.Should().Be("One or more validation errors occurred.");
        error.Errors["GitHubUsername"][0].Should().Be($"There is no GitHub user with username {invalidData}");
    }
}
