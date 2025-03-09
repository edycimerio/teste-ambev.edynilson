using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Command to update an existing sale
/// </summary>
public class UpdateSaleCommand : IRequest<UpdateSaleResult>
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public required string Number { get; set; }

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
    public required List<UpdateSaleItemCommand> Items { get; set; }
}

/// <summary>
/// Command to update a sale item
/// </summary>
public class UpdateSaleItemCommand
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
