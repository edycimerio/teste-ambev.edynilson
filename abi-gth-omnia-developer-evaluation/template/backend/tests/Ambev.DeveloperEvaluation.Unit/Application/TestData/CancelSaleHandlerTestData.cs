using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Contains test data for CancelSaleHandler tests.
/// </summary>
public static class CancelSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid CancelSaleCommand for testing.
    /// </summary>
    /// <returns>A valid CancelSaleCommand.</returns>
    public static CancelSaleCommand GenerateValidCommand()
    {
        return new CancelSaleCommand
        {
            Number = "SALE-20250309133420"
        };
    }

    /// <summary>
    /// Generates an invalid CancelSaleCommand for testing.
    /// </summary>
    /// <returns>An invalid CancelSaleCommand.</returns>
    public static CancelSaleCommand GenerateInvalidCommand()
    {
        return new CancelSaleCommand
        {
            Number = string.Empty
        };
    }
}
