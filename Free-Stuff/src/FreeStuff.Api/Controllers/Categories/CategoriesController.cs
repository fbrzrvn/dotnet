using FreeStuff.Categories.Application.Create;
using FreeStuff.Categories.Application.Get;
using FreeStuff.Categories.Application.GetAll;
using FreeStuff.Categories.Application.Update;
using FreeStuff.Contracts.Categories.Requests;
using FreeStuff.Contracts.Categories.Responses;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreeStuff.Api.Controllers.Categories;

public class CategoriesController : ApiController
{
    private readonly ISender _bus;
    private readonly IMapper _mapper;

    public CategoriesController(ISender bus, IMapper mapper)
    {
        _bus    = bus;
        _mapper = mapper;
    }

    [HttpPost(ApiEndpoints.Category.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<CreateCategoryCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            category => CreatedAtRoute(
                "GetCategory",
                new { id = category.Id },
                _mapper.Map<CategoryResponse>(category)
            ),
            errors => Problem(errors)
        );
    }

    [HttpGet(ApiEndpoints.Category.Get, Name = "GetCategory")]
    public async Task<IActionResult> Get([FromRoute] string name, CancellationToken cancellationToken)
    {
        var query  = new GetCategoryQuery(name);
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors)
        );
    }

    [HttpGet(ApiEndpoints.Category.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query  = new GetAllCategoriesQuery();
        var result = await _bus.Send(query, cancellationToken);

        return result.Match(
            categories => Ok(_mapper.Map<List<CategoryResponse>>(categories)),
            errors => Problem(errors)
        );
    }

    [HttpPut(ApiEndpoints.Category.Update)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<UpdateCategoryCommand>(request);
        var result  = await _bus.Send(command, cancellationToken);

        return result.Match(
            category => Ok(_mapper.Map<CategoryResponse>(category)),
            errors => Problem(errors)
        );
    }
}
