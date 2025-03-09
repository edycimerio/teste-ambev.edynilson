namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Represents the response when listing sales.
/// </summary>
public class ListSalesResponse
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public required List<SaleResponse> Sales { get; set; }
}

/// <summary>
/// Represents a sale in the list sales response.
/// </summary>
public class SaleResponse
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    public required DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public required string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the customer document.
    /// </summary>
    public required string CustomerDocument { get; set; }

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    public required decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is canceled.
    /// </summary>
    public required bool IsCanceled { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the sale.
    /// </summary>
    public required int ItemCount { get; set; }
}
