using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route(ApiEndpoints.Base)]
public class PostsController : ControllerBase
{
    [HttpGet(ApiEndpoints.Id)]
    public IActionResult GetById([FromRoute] Guid id)
    {
        Post post = new() { Id = id, Text = "Hello world from v1" };

        return Ok(post);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Post> posts = new()
        {
            new Post { Id = Guid.NewGuid(), Text = "Hello world from v1" },
            new Post { Id = Guid.NewGuid(), Text = "Hola mundo desde v1" },
            new Post { Id = Guid.NewGuid(), Text = "Ciao mondo da v1" }
        };

        return Ok(posts);
    }
}