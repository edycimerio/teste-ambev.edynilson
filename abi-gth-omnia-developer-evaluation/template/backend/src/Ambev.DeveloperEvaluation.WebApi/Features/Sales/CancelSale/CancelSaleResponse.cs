namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// Represents the response after canceling a sale.
/// </summary>
public class CancelSaleResponse
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Gets or sets the cancellation date.
    /// </summary>
    public required DateTime CancellationDate { get; set; }
}
