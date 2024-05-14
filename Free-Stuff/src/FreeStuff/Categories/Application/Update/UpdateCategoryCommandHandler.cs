using ErrorOr;
using FreeStuff.Categories.Application.Shared.Dto;
using FreeStuff.Categories.Domain.Errors;
using FreeStuff.Categories.Domain.Ports;
using MapsterMapper;
using MediatR;

namespace FreeStuff.Categories.Application.Update;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ErrorOr<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper             _mapper;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper             = mapper;
    }

    public async Task<ErrorOr<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.Name, cancellationToken);

        if (category is null)
        {
            return Errors.Category.NotFound(request.Name);
        }

        category.Update(request.NewName, request.Description);

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<CategoryDto>(category);

        return result;
    }
}
