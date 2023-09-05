using Back.Application.Models;
using FluentValidation;

namespace Back.Application.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty();
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Product name cannot be empty.");
    }
}
