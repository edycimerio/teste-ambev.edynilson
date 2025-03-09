namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Represents a request to list sales.
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
