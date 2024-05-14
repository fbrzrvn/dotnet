using FluentValidation;

namespace FreeStuff.Categories.Application.Update;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(request => request.Name).NotEmpty();
        RuleFor(request => request.NewName).NotEmpty().NotEqual(request => request.Name);
        RuleFor(request => request.Description).NotEmpty();
    }
}
