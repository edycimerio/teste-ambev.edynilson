namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Result for the ListSales operation.
/// </summary>
public class ListSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public List<SaleResult> Sales { get; set; } = new();
}

/// <summary>
/// Result for a sale in the ListSales operation.
/// </summary>
public class SaleResult
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sale date.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer document.
    /// </summary>
    public string CustomerDocument { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total amount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether the sale is canceled.
    /// </summary>
    public bool IsCanceled { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the sale.
    /// </summary>
    public int ItemCount { get; set; }
}
