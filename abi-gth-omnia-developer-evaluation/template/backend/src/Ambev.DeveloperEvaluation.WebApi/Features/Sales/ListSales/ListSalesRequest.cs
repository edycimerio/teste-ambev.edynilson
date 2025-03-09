namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// API request model for ListSales operation
/// </summary>
public class ListSalesRequest
{
    /// <summary>
    /// Gets or sets the start date for filtering sales.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date for filtering sales.
    /// </summary>
    public DateTime? EndDate { get; set; }
}
