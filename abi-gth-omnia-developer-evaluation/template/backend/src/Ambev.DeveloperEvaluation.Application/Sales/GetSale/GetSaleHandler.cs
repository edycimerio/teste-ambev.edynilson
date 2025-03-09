using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult?>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<GetSaleResult?> Handle(GetSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByNumberAsync(request.Number, cancellationToken);
        if (sale == null)
            return null;

        return new GetSaleResult
        {
            Number = sale.Number,
            SaleDate = sale.SaleDate,
            CustomerName = sale.CustomerName,
            CustomerDocument = sale.CustomerDocument,
            TotalAmount = sale.TotalAmount,
            IsCanceled = sale.IsCanceled,
            Items = sale.Items.Select(i => new GetSaleItemResult
            {
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }
}
