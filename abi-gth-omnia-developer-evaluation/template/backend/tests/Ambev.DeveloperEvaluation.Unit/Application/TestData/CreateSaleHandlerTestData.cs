using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Contains test data for CreateSaleHandler tests.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid CreateSaleCommand for testing.
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return new CreateSaleCommand
        {
            CustomerName = "Cliente Teste",
            CustomerDocument = "12345678900",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductName = "Produto Teste 1",
                    ProductCode = "PROD1",
                    Quantity = 10,
                    UnitPrice = 100.00m
                }
            }
        };
    }

    /// <summary>
    /// Generates a CreateSaleCommand with invalid data for testing.
    /// </summary>
    public static CreateSaleCommand GenerateInvalidCommand()
    {
        return new CreateSaleCommand
        {
            CustomerName = "", // Inválido: vazio
            CustomerDocument = "", // Inválido: vazio
            Items = new List<CreateSaleItemCommand>() // Inválido: sem itens
        };
    }

    /// <summary>
    /// Generates a CreateSaleCommand with multiple items for testing discount rules.
    /// </summary>
    public static CreateSaleCommand GenerateCommandWithMultipleItems()
    {
        return new CreateSaleCommand
        {
            CustomerName = "Cliente Teste",
            CustomerDocument = "12345678900",
            Items = new List<CreateSaleItemCommand>
            {
                new()
                {
                    ProductName = "Produto Teste 1",
                    ProductCode = "PROD1",
                    Quantity = 5,
                    UnitPrice = 100.00m
                },
                new()
                {
                    ProductName = "Produto Teste 2",
                    ProductCode = "PROD2",
                    Quantity = 5,
                    UnitPrice = 150.00m
                }
            }
        };
    }
}
