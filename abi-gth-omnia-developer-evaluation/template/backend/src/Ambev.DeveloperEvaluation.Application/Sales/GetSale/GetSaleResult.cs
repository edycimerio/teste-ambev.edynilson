using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result for the GetSale operation.
/// </summary>
public class GetSaleResult
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
    /// Gets or sets the sale items.
    /// </summary>
    public List<GetSaleItemResult> Items { get; set; } = new();
}

/// <summary>
/// Result for a sale item in the GetSale operation.
/// </summary>
public class GetSaleItemResult
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product code.
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total price after discount.
    /// </summary>
    public decimal TotalPrice { get; set; }
}
