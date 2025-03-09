using AutoMapper;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSale commands.
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CancelSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByNumberAsync(request.Number, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with number {request.Number} not found");

        if (sale.IsCanceled)
            throw new InvalidOperationException($"Sale {request.Number} is already canceled");

        sale.IsCanceled = true;
        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<CancelSaleResult>(sale);
    }
}
