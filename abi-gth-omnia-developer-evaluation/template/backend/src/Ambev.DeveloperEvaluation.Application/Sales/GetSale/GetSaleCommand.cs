namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for getting a sale by its number
/// </summary>
public class GetSaleCommand
{
    /// <summary>
    /// Gets or sets the sale number
    /// </summary>
    public required string Number { get; set; }
}
