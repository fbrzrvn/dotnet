using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PostsController : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        Post post = new() { Id = id, Text = "Hello world from v1" };

        return Ok(post);
    }
}