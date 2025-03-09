using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSale commands.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = new Sale
        {
            CustomerName = request.CustomerName,
            CustomerDocument = request.CustomerDocument
        };

        foreach (var item in request.Items)
        {
            var saleItem = new SaleItem
            {
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            // Calculate discount based on quantity
            if (item.Quantity >= 10)
                saleItem.Discount = 20; // 20% discount
            else if (item.Quantity >= 4)
                saleItem.Discount = 10; // 10% discount

            saleItem.RecalculateTotalPrice();
            sale.Items.Add(saleItem);
        }

        sale.RecalculateTotalAmount();

        await _saleRepository.CreateAsync(sale, cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
