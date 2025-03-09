using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the GetSaleHandler class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the GetSaleHandlerTests class.
    /// Sets up the test dependencies.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale request returns the sale details.
    /// </summary>
    [Fact(DisplayName = "Given valid sale number When getting sale Then returns sale details")]
    public async Task Handle_ValidRequest_ReturnsSaleDetails()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 800.00m,
            IsCanceled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Produto 1",
                    ProductCode = "P1",
                    Quantity = 10,
                    UnitPrice = 100.00m,
                    Discount = 20,
                    TotalPrice = 800.00m
                }
            }
        };

        var result = new GetSaleResult
        {
            Number = existingSale.Number,
            SaleDate = existingSale.SaleDate,
            CustomerName = existingSale.CustomerName,
            CustomerDocument = existingSale.CustomerDocument,
            TotalAmount = existingSale.TotalAmount,
            IsCanceled = existingSale.IsCanceled,
            Items = existingSale.Items.Select(i => new GetSaleItemResult
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

        _mapper.Map<GetSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Number.Should().Be(existingSale.Number);
        getSaleResult.CustomerName.Should().Be(existingSale.CustomerName);
        getSaleResult.CustomerDocument.Should().Be(existingSale.CustomerDocument);
        getSaleResult.TotalAmount.Should().Be(800.00m);
        getSaleResult.IsCanceled.Should().BeFalse();
        getSaleResult.Items.Should().HaveCount(1);
        getSaleResult.Items[0].ProductName.Should().Be("Produto 1");
        getSaleResult.Items[0].ProductCode.Should().Be("P1");
        getSaleResult.Items[0].Quantity.Should().Be(10);
        getSaleResult.Items[0].UnitPrice.Should().Be(100.00m);
        getSaleResult.Items[0].Discount.Should().Be(20);
        getSaleResult.Items[0].TotalPrice.Should().Be(800.00m);
    }

    /// <summary>
    /// Tests that an invalid sale request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale number When getting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that getting a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When getting Then throws not found exception")]
    public async Task Handle_NonExistentSale_ThrowsNotFoundException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Sale?>(null));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with number {command.Number} not found");
    }

    /// <summary>
    /// Tests that getting a canceled sale returns correct status.
    /// </summary>
    [Fact(DisplayName = "Given canceled sale When getting Then returns canceled status")]
    public async Task Handle_CanceledSale_ReturnsCanceledStatus()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 800.00m,
            IsCanceled = true,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Produto 1",
                    ProductCode = "P1",
                    Quantity = 10,
                    UnitPrice = 100.00m,
                    Discount = 20,
                    TotalPrice = 800.00m
                }
            }
        };

        var result = new GetSaleResult
        {
            Number = existingSale.Number,
            SaleDate = existingSale.SaleDate,
            CustomerName = existingSale.CustomerName,
            CustomerDocument = existingSale.CustomerDocument,
            TotalAmount = existingSale.TotalAmount,
            IsCanceled = true,
            Items = existingSale.Items.Select(i => new GetSaleItemResult
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

        _mapper.Map<GetSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Number.Should().Be(existingSale.Number);
        getSaleResult.IsCanceled.Should().BeTrue();
    }
}
