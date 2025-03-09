namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale
/// </summary>
public class CreateSaleCommand
{
    /// <summary>
    /// Gets or sets the customer name
    /// </summary>
    public required string CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the customer document
    /// </summary>
    public required string CustomerDocument { get; set; }

    /// <summary>
    /// Gets or sets the sale items
    /// </summary>
    public required List<CreateSaleItemCommand> Items { get; set; }
}

/// <summary>
/// Command for creating a sale item
/// </summary>
public class CreateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// Gets or sets the product code
    /// </summary>
    public required string ProductCode { get; set; }

    /// <summary>
    /// Gets or sets the quantity
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price
    /// </summary>
    public required decimal UnitPrice { get; set; }
}
