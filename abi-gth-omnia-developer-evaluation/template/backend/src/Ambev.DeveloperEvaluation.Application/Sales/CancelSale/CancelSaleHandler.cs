using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, ValidationResultDetail>
{
    private readonly ISaleRepository _saleRepository;

    public CancelSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ValidationResultDetail> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByNumberAsync(request.Number, cancellationToken);
        if (sale == null)
        {
            return new ValidationResultDetail
            {
                IsValid = false,
                Errors = new[] { new ValidationErrorDetail("Number", "Sale not found") }
            };
        }

        if (sale.IsCanceled)
        {
            return new ValidationResultDetail
            {
                IsValid = false,
                Errors = new[] { new ValidationErrorDetail("Number", "Sale is already canceled") }
            };
        }

        sale.Cancel();
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return new ValidationResultDetail { IsValid = true };
    }
}
