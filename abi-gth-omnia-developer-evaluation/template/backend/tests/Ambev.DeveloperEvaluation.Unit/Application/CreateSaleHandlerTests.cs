using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CreateSaleHandlerTests class.
    /// Sets up the test dependencies.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = "SALE-20250309133420",
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            SaleDate = DateTime.UtcNow,
            TotalAmount = 800.00m, // 10 itens * 100.00 - 20% desconto
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice,
                    Discount = 20, // 20% por ter 10 itens
                    TotalPrice = 800.00m
                }
            }
        };

        var result = new CreateSaleResult
        {
            Number = sale.Number,
            SaleDate = sale.SaleDate,
            CustomerName = sale.CustomerName,
            CustomerDocument = sale.CustomerDocument,
            TotalAmount = sale.TotalAmount,
            Items = sale.Items.Select(i => new CreateSaleItemResult
            {
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        _mapper.Map<Sale>(command).Returns(new Sale
        {
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            Items = new List<SaleItem>
            {
                new()
                {
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice
                }
            }
        });

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        _mapper.Map<CreateSaleResult>(sale).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Number.Should().Be(sale.Number);
        createSaleResult.TotalAmount.Should().Be(800.00m);
        createSaleResult.Items.Should().HaveCount(1);
        createSaleResult.Items[0].Discount.Should().Be(20);
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.Items.All(i => i.Discount == 20)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the discount is correctly applied when quantity is 10 or more.
    /// </summary>
    [Fact(DisplayName = "Given sale with 10 or more items When creating Then applies 20% discount")]
    public async Task Handle_TenOrMoreItems_AppliesTwentyPercentDiscount()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand(); // Já vem com 10 itens
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = "SALE-20250309133420",
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            SaleDate = DateTime.UtcNow,
            TotalAmount = 800.00m,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice,
                    Discount = 20,
                    TotalPrice = 800.00m
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(new Sale
        {
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            Items = new List<SaleItem>
            {
                new()
                {
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice
                }
            }
        });

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.Items.All(i => i.Discount == 20) && 
                s.TotalAmount == 800.00m),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that multiple items are correctly handled and mapped.
    /// </summary>
    [Fact(DisplayName = "Given sale with multiple items When creating Then handles all items correctly")]
    public async Task Handle_MultipleItems_HandlesAllItemsCorrectly()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithMultipleItems();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = "SALE-20250309133420",
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            SaleDate = DateTime.UtcNow,
            TotalAmount = 1125.00m, // (5 * 100 - 10%) + (5 * 150 - 10%)
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice,
                    Discount = 10,
                    TotalPrice = 450.00m // 5 * 100 - 10%
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = command.Items[1].ProductName,
                    ProductCode = command.Items[1].ProductCode,
                    Quantity = command.Items[1].Quantity,
                    UnitPrice = command.Items[1].UnitPrice,
                    Discount = 10,
                    TotalPrice = 675.00m // 5 * 150 - 10%
                }
            }
        };

        _mapper.Map<Sale>(command).Returns(new Sale
        {
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            Items = new List<SaleItem>
            {
                new()
                {
                    ProductName = command.Items[0].ProductName,
                    ProductCode = command.Items[0].ProductCode,
                    Quantity = command.Items[0].Quantity,
                    UnitPrice = command.Items[0].UnitPrice
                },
                new()
                {
                    ProductName = command.Items[1].ProductName,
                    ProductCode = command.Items[1].ProductCode,
                    Quantity = command.Items[1].Quantity,
                    UnitPrice = command.Items[1].UnitPrice
                }
            }
        });

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(sale));

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.Items.Count == 2 &&
                s.TotalAmount == 1125.00m &&
                s.Items.All(i => i.Discount == 10)),
            Arg.Any<CancellationToken>());
    }
}
