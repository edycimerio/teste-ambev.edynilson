using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the ListSalesHandler class.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the ListSalesHandlerTests class.
    /// Sets up the test dependencies.
    /// </summary>
    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that the handler returns all sales when there are sales in the repository.
    /// </summary>
    [Fact(DisplayName = "Given existing sales When listing Then returns all sales")]
    public async Task Handle_ExistingSales_ReturnsAllSales()
    {
        // Given
        var command = new ListSalesCommand();
        var existingSales = new List<Sale>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Number = "SALE-20250309133420",
                CustomerName = "Nome do Cliente 1",
                CustomerDocument = "Documento do Cliente 1",
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
            },
            new()
            {
                Id = Guid.NewGuid(),
                Number = "SALE-20250309133421",
                CustomerName = "Nome do Cliente 2",
                CustomerDocument = "Documento do Cliente 2",
                SaleDate = DateTime.UtcNow,
                TotalAmount = 1125.00m,
                IsCanceled = true,
                Items = new List<SaleItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Produto 2",
                        ProductCode = "P2",
                        Quantity = 5,
                        UnitPrice = 150.00m,
                        Discount = 10,
                        TotalPrice = 675.00m
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ProductName = "Produto 3",
                        ProductCode = "P3",
                        Quantity = 5,
                        UnitPrice = 100.00m,
                        Discount = 10,
                        TotalPrice = 450.00m
                    }
                }
            }
        };

        var result = new ListSalesResult
        {
            Sales = existingSales.Select(s => new SaleResult
            {
                Number = s.Number,
                SaleDate = s.SaleDate,
                CustomerName = s.CustomerName,
                CustomerDocument = s.CustomerDocument,
                TotalAmount = s.TotalAmount,
                IsCanceled = s.IsCanceled,
                ItemCount = s.Items.Count
            }).ToList()
        };

        _saleRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(existingSales.AsEnumerable());

        _mapper.Map<List<SaleResult>>(Arg.Any<IEnumerable<Sale>>()).Returns(result.Sales);

        // When
        var listSalesResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        listSalesResult.Should().NotBeNull();
        listSalesResult.Sales.Should().HaveCount(2);

        // Verificar primeira venda
        var firstSale = listSalesResult.Sales[0];
        firstSale.Number.Should().Be("SALE-20250309133420");
        firstSale.CustomerName.Should().Be("Nome do Cliente 1");
        firstSale.TotalAmount.Should().Be(800.00m);
        firstSale.IsCanceled.Should().BeFalse();
        firstSale.ItemCount.Should().Be(1);

        // Verificar segunda venda
        var secondSale = listSalesResult.Sales[1];
        secondSale.Number.Should().Be("SALE-20250309133421");
        secondSale.CustomerName.Should().Be("Nome do Cliente 2");
        secondSale.TotalAmount.Should().Be(1125.00m);
        secondSale.IsCanceled.Should().BeTrue();
        secondSale.ItemCount.Should().Be(2);
    }

    /// <summary>
    /// Tests that the handler returns empty list when there are no sales in the repository.
    /// </summary>
    [Fact(DisplayName = "Given no sales When listing Then returns empty list")]
    public async Task Handle_NoSales_ReturnsEmptyList()
    {
        // Given
        var command = new ListSalesCommand();
        var emptySales = new List<Sale>();
        var emptyResult = new List<SaleResult>();

        _saleRepository.ListAsync(Arg.Any<CancellationToken>())
            .Returns(emptySales.AsEnumerable());

        _mapper.Map<List<SaleResult>>(Arg.Any<IEnumerable<Sale>>()).Returns(emptyResult);

        // When
        var listSalesResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        listSalesResult.Should().NotBeNull();
        listSalesResult.Sales.Should().BeEmpty();
    }
}
