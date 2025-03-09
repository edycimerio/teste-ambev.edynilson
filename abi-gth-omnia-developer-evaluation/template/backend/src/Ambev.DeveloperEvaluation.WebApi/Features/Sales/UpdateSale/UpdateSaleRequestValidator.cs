using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleRequest
/// </summary>
public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
{
    public UpdateSaleRequestValidator()
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
            .ChildRules(item =>
            {
                item.RuleFor(x => x.ProductName)
                    .NotEmpty()
                    .WithMessage("Product name is required")
                    .MaximumLength(100)
                    .WithMessage("Product name cannot exceed 100 characters");

                item.RuleFor(x => x.ProductCode)
                    .NotEmpty()
                    .WithMessage("Product code is required")
                    .MaximumLength(20)
                    .WithMessage("Product code cannot exceed 20 characters");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero")
                    .LessThanOrEqualTo(20)
                    .WithMessage("Maximum quantity per item is 20");

                item.RuleFor(x => x.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than zero");
            });
    }
}
