using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    public GetAllMoviesOptionsValidator()
    {
        RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.SortField).Must(x => x is null || AcceptableSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("You cn only sort by 'title' or 'yearofrelease'");

        RuleFor(x => x.Limit).InclusiveBetween(1, 25).WithMessage("You can get between 1 and 25 movies per page");
    }

    private static readonly string[] AcceptableSortFields =
    {
        "title", "yearofrelease"
    };
}
