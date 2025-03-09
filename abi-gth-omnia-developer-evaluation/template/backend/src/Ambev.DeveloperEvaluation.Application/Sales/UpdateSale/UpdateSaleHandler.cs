using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSale commands.
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByNumberAsync(request.Number, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with number {request.Number} not found");

        if (sale.IsCanceled)
            throw new InvalidOperationException($"Sale {request.Number} is canceled and cannot be updated");

        sale.CustomerName = request.CustomerName;
        sale.CustomerDocument = request.CustomerDocument;
        sale.Items.Clear();

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

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}
