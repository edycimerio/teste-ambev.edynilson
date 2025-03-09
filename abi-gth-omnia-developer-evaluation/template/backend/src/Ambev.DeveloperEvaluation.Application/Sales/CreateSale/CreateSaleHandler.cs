using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, ValidationResultDetail>
{
    private readonly ISaleRepository _saleRepository;

    public CreateSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ValidationResultDetail> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = new Sale(request.CustomerName, request.CustomerDocument);

        foreach (var item in request.Items)
        {
            sale.AddItem(item.ProductName, item.ProductCode, item.UnitPrice, item.Quantity);
        }

        var validationResult = await sale.ValidateAsync();
        if (validationResult.Any())
        {
            return new ValidationResultDetail
            {
                IsValid = false,
                Errors = validationResult
            };
        }

        await _saleRepository.CreateAsync(sale, cancellationToken);

        return new ValidationResultDetail { IsValid = true };
    }
}
