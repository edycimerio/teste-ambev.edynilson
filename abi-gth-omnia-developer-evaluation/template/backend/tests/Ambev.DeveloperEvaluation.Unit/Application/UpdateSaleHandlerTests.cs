using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the UpdateSaleHandler class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the UpdateSaleHandlerTests class.
    /// Sets up the test dependencies.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Old Customer Name",
            CustomerDocument = "Old Document",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 1000.00m,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Old Product",
                    ProductCode = "OLD1",
                    Quantity = 5,
                    UnitPrice = 200.00m,
                    Discount = 0,
                    TotalPrice = 1000.00m
                }
            }
        };

        var updatedSale = new Sale
        {
            Id = existingSale.Id,
            Number = existingSale.Number,
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            SaleDate = existingSale.SaleDate,
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

        var result = new UpdateSaleResult
        {
            Number = updatedSale.Number,
            SaleDate = updatedSale.SaleDate,
            CustomerName = updatedSale.CustomerName,
            CustomerDocument = updatedSale.CustomerDocument,
            TotalAmount = updatedSale.TotalAmount,
            Items = updatedSale.Items.Select(i => new UpdateSaleItemResult
            {
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingSale));

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(updatedSale));

        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateSaleResult.Should().NotBeNull();
        updateSaleResult.Number.Should().Be(existingSale.Number);
        updateSaleResult.TotalAmount.Should().Be(800.00m);
        updateSaleResult.Items.Should().HaveCount(1);
        updateSaleResult.Items[0].Discount.Should().Be(20);
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => 
                s.Id == existingSale.Id &&
                s.Number == existingSale.Number &&
                s.CustomerName == command.CustomerName &&
                s.CustomerDocument == command.CustomerDocument &&
                s.Items.All(i => i.Discount == 20)),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When updating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that updating a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When updating Then throws not found exception")]
    public async Task Handle_NonExistentSale_ThrowsNotFoundException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Sale?>(null));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with number {command.Number} not found");
    }

    /// <summary>
    /// Tests that updating a canceled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given canceled sale When updating Then throws invalid operation exception")]
    public async Task Handle_CanceledSale_ThrowsInvalidOperationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Old Customer Name",
            CustomerDocument = "Old Document",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            IsCanceled = true,
            TotalAmount = 1000.00m,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Old Product",
                    ProductCode = "OLD1",
                    Quantity = 5,
                    UnitPrice = 200.00m,
                    Discount = 0,
                    TotalPrice = 1000.00m
                }
            }
        };

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingSale));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Sale {command.Number} is canceled and cannot be updated");
    }

    /// <summary>
    /// Tests that multiple items are correctly handled and mapped during update.
    /// </summary>
    [Fact(DisplayName = "Given sale with multiple items When updating Then handles all items correctly")]
    public async Task Handle_MultipleItems_HandlesAllItemsCorrectly()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateCommandWithMultipleItems();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Old Customer Name",
            CustomerDocument = "Old Document",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 1000.00m,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Old Product",
                    ProductCode = "OLD1",
                    Quantity = 5,
                    UnitPrice = 200.00m,
                    Discount = 0,
                    TotalPrice = 1000.00m
                }
            }
        };

        var updatedSale = new Sale
        {
            Id = existingSale.Id,
            Number = existingSale.Number,
            CustomerName = command.CustomerName,
            CustomerDocument = command.CustomerDocument,
            SaleDate = existingSale.SaleDate,
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

        var result = new UpdateSaleResult
        {
            Number = updatedSale.Number,
            SaleDate = updatedSale.SaleDate,
            CustomerName = updatedSale.CustomerName,
            CustomerDocument = updatedSale.CustomerDocument,
            TotalAmount = updatedSale.TotalAmount,
            Items = updatedSale.Items.Select(i => new UpdateSaleItemResult
            {
                ProductName = i.ProductName,
                ProductCode = i.ProductCode,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingSale));

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(updatedSale));

        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateSaleResult.Should().NotBeNull();
        updateSaleResult.Number.Should().Be(existingSale.Number);
        updateSaleResult.TotalAmount.Should().Be(1125.00m);
        updateSaleResult.Items.Should().HaveCount(2);
        updateSaleResult.Items.Should().AllSatisfy(i => i.Discount.Should().Be(10));
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => 
                s.Id == existingSale.Id &&
                s.Number == existingSale.Number &&
                s.CustomerName == command.CustomerName &&
                s.CustomerDocument == command.CustomerDocument &&
                s.Items.Count == 2 &&
                s.TotalAmount == 1125.00m &&
                s.Items.All(i => i.Discount == 10)),
            Arg.Any<CancellationToken>());
    }
}
