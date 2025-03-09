using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateSaleResult>> CreateSale([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{number}")]
    public async Task<ActionResult<GetSaleResult>> GetSale([FromRoute] string number, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSaleCommand { Number = number }, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ListSalesResult>> ListSales(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListSalesCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{number}")]
    public async Task<ActionResult<UpdateSaleResult>> UpdateSale([FromRoute] string number, [FromBody] UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        command.Number = number;
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{number}/cancel")]
    public async Task<ActionResult> CancelSale([FromRoute] string number, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelSaleCommand { Number = number }, cancellationToken);
        return NoContent();
    }
}
