using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PostsController : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        Post post = new() { Id = id, Text = "Hello world from v2" };

        return Ok(post);
    }
}