using FluentValidation;

namespace FreeStuff.Categories.Application.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(100);
        RuleFor(request => request.Description)
            .Cascade(CascadeMode.Continue)
            .MaximumLength(500)
            .When(request => !string.IsNullOrWhiteSpace(request.Description));
    }
}
