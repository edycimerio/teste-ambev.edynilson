using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Contains test data for GetSaleHandler tests.
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid GetSaleCommand for testing.
    /// </summary>
    /// <returns>A valid GetSaleCommand.</returns>
    public static GetSaleCommand GenerateValidCommand()
    {
        return new GetSaleCommand
        {
            Number = "SALE-20250309133420"
        };
    }

    /// <summary>
    /// Generates an invalid GetSaleCommand for testing.
    /// </summary>
    /// <returns>An invalid GetSaleCommand.</returns>
    public static GetSaleCommand GenerateInvalidCommand()
    {
        return new GetSaleCommand
        {
            Number = string.Empty
        };
    }
}
