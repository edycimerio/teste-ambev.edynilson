using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers;

/// <summary>
/// Controller for managing sales operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="command">Sale creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    /// <response code="200">Sale created successfully</response>
    /// <response code="400">Invalid data provided</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsValid)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Gets a sale by its number
    /// </summary>
    /// <param name="number">Sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found</returns>
    /// <response code="200">Sale found</response>
    /// <response code="404">Sale not found</response>
    [HttpGet("{number}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNumber([FromRoute] string number, CancellationToken cancellationToken)
    {
        var command = new GetSaleCommand { Number = number };
        var result = await _mediator.Send(command, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Cancels a sale
    /// </summary>
    /// <param name="number">Sale number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    /// <response code="200">Sale canceled successfully</response>
    /// <response code="400">Invalid operation</response>
    /// <response code="404">Sale not found</response>
    [HttpPost("{number}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] string number, CancellationToken cancellationToken)
    {
        var command = new CancelSaleCommand { Number = number };
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsValid)
        {
            if (result.Errors.Any(e => e.Message == "Sale not found"))
                return NotFound();

            return BadRequest(result);
        }

        return Ok(result);
    }
}
