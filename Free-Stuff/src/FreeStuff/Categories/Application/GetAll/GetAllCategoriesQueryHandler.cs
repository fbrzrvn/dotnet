using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Categories.Application.GetAll;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, ErrorOr<List<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper             _mapper;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper             = mapper;
    }

    public async Task<ErrorOr<List<CategoryDto>>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken
            cancellationToken
    )
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken) ?? new List<Category>();

        return _mapper.Map<List<CategoryDto>>(categories);
    }
}
