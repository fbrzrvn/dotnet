using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[ApiController]
[Route("customers/{id:guid}/image")]
public class CustomerImageController : ControllerBase
{
    private readonly ICustomerImageService _customerImageService;

    public CustomerImageController(ICustomerImageService customerImageService)
    {
        _customerImageService = customerImageService;
    }

    [HttpPost("")]
    public async Task<IActionResult> Upload([FromRoute] Guid id, [FromForm(Name = "Data")] IFormFile file)
    {
        PutObjectResponse response = await _customerImageService.UploadImageAsync(id, file);

        if (response.HttpStatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpGet("")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        try
        {
            GetObjectResponse? response = await _customerImageService.GetImageAsync(id);

            return File(response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.")
        {
            return NotFound();
        }
    }

    [HttpDelete("")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        DeleteObjectResponse response = await _customerImageService.DeleteImageAsync(id);

        return response.HttpStatusCode switch
        {
            HttpStatusCode.NoContent => Ok(),
            HttpStatusCode.NotFound  => NotFound(),
            _                        => BadRequest()
        };
    }
}