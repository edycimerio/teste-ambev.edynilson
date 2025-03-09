namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public required string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the customer document.
    /// </summary>
    public required string CustomerDocument { get; set; }

    /// <summary>
    /// Gets or sets the sale items.
    /// </summary>
    public required List<CreateSaleItemRequest> Items { get; set; }
}

/// <summary>
/// Represents a sale item in the create sale request.
/// </summary>
public class CreateSaleItemRequest
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
}
