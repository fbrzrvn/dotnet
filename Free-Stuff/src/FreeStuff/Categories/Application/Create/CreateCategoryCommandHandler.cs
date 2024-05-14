using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain;
using FreeStuff.Categories.Domain.Ports;
using FreeStuff.Categories.Domain.Errors;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Categories.Application.Create;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper             _mapper;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper             = mapper;
    }

    public async Task<ErrorOr<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetAsync(request.Name, cancellationToken);

        if (existingCategory is not null)
        {
            return Errors.Category.DuplicateCategoryName(request.Name);
        }

        var category = string.IsNullOrEmpty(request.Description)
            ? Category.Create(request.Name)
            : Category.Create(request.Name, request.Description);

        await _categoryRepository.CreateAsync(category, cancellationToken);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CategoryDto>(category);

        return result;
    }
}
