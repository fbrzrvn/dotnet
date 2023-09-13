using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using Customers.Api.Domain;
using Customers.Api.Mapping;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("customers")]
    public async Task<IActionResult> Create([FromBody] CustomerRequest request)
    {
        Customer customer = request.ToCustomer();

        await _customerService.CreateAsync(customer);

        CustomerResponse customerResponse = customer.ToCustomerResponse();

        return CreatedAtAction("Get", new { customerResponse.Id }, customerResponse);
    }

    [HttpGet("customers/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        Customer? customer = await _customerService.GetAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        CustomerResponse customerResponse = customer.ToCustomerResponse();

        return Ok(customerResponse);
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Customer>   customers         = await _customerService.GetAllAsync();
        GetAllCustomersResponse customersResponse = customers.ToCustomersResponse();

        return Ok(customersResponse);
    }

    [HttpPut("customers/{id:guid}")]
    public async Task<IActionResult> Update
    (
        [FromMultiSource] UpdateCustomerRequest request
    )
    {
        Customer? existingCustomer = await _customerService.GetAsync(request.Id);

        if (existingCustomer is null)
        {
            return NotFound();
        }

        Customer customer = request.ToCustomer();
        await _customerService.UpdateAsync(customer);

        CustomerResponse customerResponse = customer.ToCustomerResponse();

        return Ok(customerResponse);
    }

    [HttpDelete("customers/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        bool deleted = await _customerService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}