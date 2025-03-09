using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

/// <summary>
/// Validator for the Sale entity using FluentValidation.
/// </summary>
public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required");

        RuleFor(sale => sale.CustomerDocument)
            .NotEmpty()
            .WithMessage("Customer document is required");

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleFor(sale => sale.Items)
            .Must(items => !HasDuplicateProducts(items))
            .WithMessage("Duplicate products are not allowed");

        RuleForEach(sale => sale.Items)
            .SetValidator(new SaleItemValidator());
    }

    private bool HasDuplicateProducts(List<SaleItem> items)
    {
        if (items == null || !items.Any())
            return false;

        return items
            .GroupBy(i => i.ProductCode)
            .Any(g => g.Count() > 1);
    }
}


