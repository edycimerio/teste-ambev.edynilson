using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
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
        ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Sale sale = _mapper.Map<Sale>(request);
        sale.Number = $"SALE-{DateTime.UtcNow:yyyyMMddHHmmss}";
        sale.SaleDate = DateTime.UtcNow;

        foreach (var item in sale.Items)
        {
            // Calculate discount based on quantity
            if (item.Quantity >= 10)
                item.Discount = 20; // 20% discount
            else if (item.Quantity >= 4)
                item.Discount = 10; // 10% discount

            item.RecalculateTotalPrice();
        }

        sale.RecalculateTotalAmount();

        Sale createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        return _mapper.Map<CreateSaleResult>(createdSale);
    }
}
