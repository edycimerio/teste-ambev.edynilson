using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
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
/// Contains unit tests for the CancelSaleHandler class.
/// </summary>
public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CancelSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the CancelSaleHandlerTests class.
    /// Sets up the test dependencies.
    /// </summary>
    public CancelSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CancelSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale cancellation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale number When canceling sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CancelSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 1000.00m,
            IsCanceled = false,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Produto 1",
                    ProductCode = "P1",
                    Quantity = 5,
                    UnitPrice = 200.00m,
                    Discount = 0,
                    TotalPrice = 1000.00m
                }
            }
        };

        var canceledSale = new Sale
        {
            Id = existingSale.Id,
            Number = existingSale.Number,
            CustomerName = existingSale.CustomerName,
            CustomerDocument = existingSale.CustomerDocument,
            SaleDate = existingSale.SaleDate,
            TotalAmount = existingSale.TotalAmount,
            IsCanceled = true,
            Items = existingSale.Items
        };

        var result = new CancelSaleResult
        {
            Number = canceledSale.Number,
            CancellationDate = DateTime.UtcNow
        };

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(existingSale));

        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(canceledSale));

        _mapper.Map<CancelSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var cancelSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        cancelSaleResult.Should().NotBeNull();
        cancelSaleResult.Number.Should().Be(existingSale.Number);
        cancelSaleResult.CancellationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        await _saleRepository.Received(1).UpdateAsync(
            Arg.Is<Sale>(s => 
                s.Id == existingSale.Id &&
                s.Number == existingSale.Number &&
                s.IsCanceled),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale cancellation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale number When canceling sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CancelSaleHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that canceling a non-existent sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale When canceling Then throws not found exception")]
    public async Task Handle_NonExistentSale_ThrowsNotFoundException()
    {
        // Given
        var command = CancelSaleHandlerTestData.GenerateValidCommand();

        _saleRepository.GetByNumberAsync(command.Number, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Sale?>(null));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with number {command.Number} not found");
    }

    /// <summary>
    /// Tests that canceling an already canceled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Given already canceled sale When canceling Then throws invalid operation exception")]
    public async Task Handle_AlreadyCanceledSale_ThrowsInvalidOperationException()
    {
        // Given
        var command = CancelSaleHandlerTestData.GenerateValidCommand();
        var existingSale = new Sale
        {
            Id = Guid.NewGuid(),
            Number = command.Number,
            CustomerName = "Nome do Cliente",
            CustomerDocument = "Documento do Cliente",
            SaleDate = DateTime.UtcNow.AddDays(-1),
            TotalAmount = 1000.00m,
            IsCanceled = true,
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Produto 1",
                    ProductCode = "P1",
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
            .WithMessage($"Sale {command.Number} is already canceled");
    }
}
