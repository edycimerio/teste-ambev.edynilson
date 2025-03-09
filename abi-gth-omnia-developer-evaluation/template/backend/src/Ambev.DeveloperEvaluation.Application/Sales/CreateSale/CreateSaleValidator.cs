using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required");

        RuleFor(x => x.CustomerDocument)
            .NotEmpty()
            .WithMessage("Customer document is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductName)
                    .NotEmpty()
                    .WithMessage("Product name is required");

                item.RuleFor(i => i.ProductCode)
                    .NotEmpty()
                    .WithMessage("Product code is required");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero")
                    .LessThanOrEqualTo(20)
                    .WithMessage("Maximum quantity per item is 20");

                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0)
                    .WithMessage("Unit price must be greater than zero");
            });
    }
}
