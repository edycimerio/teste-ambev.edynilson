using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for listing sales with optional date filters.
/// </summary>
/// <remarks>
/// This command is used to retrieve a list of sales with optional date range filters. 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="ListSalesResult"/>.
/// </remarks>
public class ListSalesCommand : IRequest<ListSalesResult>
{
}
