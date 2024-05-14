using ErrorOr;
using FreeStuff.Categories.Domain.Errors;
using FreeStuff.Categories.Domain.Ports;
using MediatR;

namespace FreeStuff.Categories.Application.Delete;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ErrorOr<bool>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.Name, cancellationToken);

        if (category is null)
        {
            return Errors.Category.NotFound(request.Name);
        }

        _categoryRepository.Delete(category);

        await _categoryRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
