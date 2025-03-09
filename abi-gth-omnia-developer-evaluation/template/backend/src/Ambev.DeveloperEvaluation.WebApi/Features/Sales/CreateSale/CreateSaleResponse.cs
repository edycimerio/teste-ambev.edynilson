namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents the response after creating a sale.
/// </summary>
public class CreateSaleResponse
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
    /// Gets or sets the sale items.
    /// </summary>
    public required List<CreateSaleItemResponse> Items { get; set; }
}

/// <summary>
/// Represents a sale item in the create sale response.
/// </summary>
public class CreateSaleItemResponse
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product code.
    /// </summary>
    public required string ProductCode { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public required decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount applied.
    /// </summary>
    public required decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total price after discount.
    /// </summary>
    public required decimal TotalPrice { get; set; }
}
