namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Result for the CancelSale operation.
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cancellation date.
    /// </summary>
    public DateTime CancellationDate { get; set; }
}
