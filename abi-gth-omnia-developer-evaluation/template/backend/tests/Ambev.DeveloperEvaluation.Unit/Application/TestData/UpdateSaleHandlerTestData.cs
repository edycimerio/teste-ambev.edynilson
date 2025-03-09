using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Contains test data for UpdateSaleHandler tests.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid UpdateSaleCommand for testing.
    /// </summary>
    /// <returns>A valid UpdateSaleCommand.</returns>
    public static UpdateSaleCommand GenerateValidCommand()
    {
        return new UpdateSaleCommand
        {
            Number = "SALE-20250309133420",
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            Items = new List<UpdateSaleItemCommand>
            {
                new()
                {
                    ProductName = "Produto 1",
                    ProductCode = "P1",
                    Quantity = 10,
                    UnitPrice = 100.00m
                }
            }
        };
    }

    /// <summary>
    /// Generates an invalid UpdateSaleCommand for testing.
    /// </summary>
    /// <returns>An invalid UpdateSaleCommand.</returns>
    public static UpdateSaleCommand GenerateInvalidCommand()
    {
        return new UpdateSaleCommand
        {
            Number = string.Empty,
            CustomerName = string.Empty,
            CustomerDocument = string.Empty,
            Items = new List<UpdateSaleItemCommand>()
        };
    }

    /// <summary>
    /// Generates a UpdateSaleCommand with multiple items for testing.
    /// </summary>
    /// <returns>A UpdateSaleCommand with multiple items.</returns>
    public static UpdateSaleCommand GenerateCommandWithMultipleItems()
    {
        return new UpdateSaleCommand
        {
            Number = "SALE-20250309133420",
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            Items = new List<UpdateSaleItemCommand>
            {
                new()
                {
                    ProductName = "Produto 1",
                    ProductCode = "P1",
                    Quantity = 5,
                    UnitPrice = 100.00m
                },
                new()
                {
                    ProductName = "Produto 2",
                    ProductCode = "P2",
                    Quantity = 5,
                    UnitPrice = 150.00m
                }
            }
        };
    }
}
