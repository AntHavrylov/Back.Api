using Back.Application.Models;
using FluentValidation;

namespace Back.Application.Validators;

public class GetAllProductOptionsValidator : AbstractValidator<GetAllProductsOptions>
{
    private readonly string[] AcceptableSortFields = new string[]
    {
        "name"
    };

    public GetAllProductOptionsValidator()
    {
        RuleFor(x => x.Name)
            .Length(0, 10)
            .WithMessage("Parameter length must be less than or equal 10 characters.");

        RuleFor(x => x.SortField)
            .Must(x => x is null || AcceptableSortFields.Contains(x,StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Acceptable sort fields are only: {string.Join(',', AcceptableSortFields)}");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page has to be greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1,100)
            .WithMessage("Page size has to be between 1 and 100.");
    
    }
}
