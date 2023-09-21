﻿using Customers.Api.Contracts.Data;
using Customers.Api.Contracts.Messages;
using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Messaging;
using Customers.Api.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Customers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IGitHubService      _gitHubService;
    private readonly ISnsMessenger       _snsMessenger;

    public CustomerService
    (
        ICustomerRepository customerRepository,
        IGitHubService      gitHubService,
        ISnsMessenger       snsMessenger
    )
    {
        _customerRepository = customerRepository;
        _gitHubService      = gitHubService;
        _snsMessenger       = snsMessenger;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        CustomerDto? existingUser = await _customerRepository.GetAsync(customer.Id);

        if (existingUser is not null)
        {
            string message = $"A user with id {customer.Id} already exists";
            throw new ValidationException(message, GenerateValidationError(nameof(Customer), message));
        }

        bool isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);

        if (!isValidGitHubUser)
        {
            string message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        CustomerDto customerDto = customer.ToCustomerDto();
        bool        response    = await _customerRepository.CreateAsync(customerDto);

        if (response)
        {
            await _snsMessenger.PublishMessageAsync(customer.ToCustomerCreatedMessage());
        }

        return response;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        CustomerDto? customerDto = await _customerRepository.GetAsync(id);

        return customerDto?.ToCustomer();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        IEnumerable<CustomerDto> customerDtos = await _customerRepository.GetAllAsync();

        return customerDtos.Select(x => x.ToCustomer());
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        CustomerDto customerDto = customer.ToCustomerDto();

        bool isValidGitHubUser = await _gitHubService.IsValidGitHubUser(customer.GitHubUsername);

        if (!isValidGitHubUser)
        {
            string message = $"There is no GitHub user with username {customer.GitHubUsername}";
            throw new ValidationException(message, GenerateValidationError(nameof(customer.GitHubUsername), message));
        }

        bool response = await _customerRepository.UpdateAsync(customerDto);

        if (response)
        {
            await _snsMessenger.PublishMessageAsync(customer.ToCustomerUpdatedMessage());
        }

        return response;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        bool response = await _customerRepository.DeleteAsync(id);

        if (response)
        {
            await _snsMessenger.PublishMessageAsync(new CustomerDeleted { Id = id });
        }

        return response;
    }

    private static ValidationFailure[] GenerateValidationError(string paramName, string message)
    {
        return new[] { new ValidationFailure(paramName, message) };
    }
}