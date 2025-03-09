namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for canceling a sale
/// </summary>
public class CancelSaleCommand
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public required string Number { get; set; }
}
