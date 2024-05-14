namespace Identity.Application.Commands.Delete;

using Common.Validators;
using FluentValidation;

public class DeleteIdentityUserCommandValidation : AbstractValidator<DeleteIdentityUserCommand>
{
    public DeleteIdentityUserCommandValidation()
    {
        RuleFor(request => request.UserId).Uid();
    }
}
