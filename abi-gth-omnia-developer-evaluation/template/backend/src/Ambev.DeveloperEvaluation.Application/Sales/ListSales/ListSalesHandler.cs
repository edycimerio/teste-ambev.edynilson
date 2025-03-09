using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for listing sales with optional date filtering
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
{
    private readonly ISaleRepository _saleRepository;

    public ListSalesHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.ListSalesAsync(request.StartDate, request.EndDate, cancellationToken);

        return new ListSalesResult
        {
            Sales = sales.Select(s => new SaleResult
            {
                Number = s.Number,
                SaleDate = s.SaleDate,
                CustomerName = s.CustomerName,
                CustomerDocument = s.CustomerDocument,
                TotalAmount = s.TotalAmount,
                IsCanceled = s.IsCanceled,
                ItemCount = s.Items.Count
            }).ToList()
        };
    }
}
