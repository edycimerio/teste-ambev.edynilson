using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale entity.
/// Implements validation rules using FluentValidation.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(x => x.Number)
            .NotEmpty()
            .WithMessage("Sale number is required")
            .MaximumLength(20)
            .WithMessage("Sale number cannot exceed 20 characters");

        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required")
            .MaximumLength(100)
            .WithMessage("Customer name cannot exceed 100 characters");

        RuleFor(x => x.CustomerDocument)
            .NotEmpty()
            .WithMessage("Customer document is required")
            .MaximumLength(20)
            .WithMessage("Customer document cannot exceed 20 characters");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required")
            .Must(items => items.Count <= 20)
            .WithMessage("A sale cannot have more than 20 items")
            .Must(items => !items.GroupBy(i => i.ProductCode).Any(g => g.Count() > 1))
            .WithMessage("Duplicate products are not allowed");

        RuleForEach(x => x.Items)
            .SetValidator(new SaleItemValidator());
    }
}
