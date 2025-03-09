namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Result containing the list of sales
/// </summary>
public class ListSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public required List<SaleResult> Sales { get; set; }
}

/// <summary>
/// Represents a sale in the list result
/// </summary>
public class SaleResult
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
