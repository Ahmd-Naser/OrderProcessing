using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Products.Commands.CreateProduct;
using OrderProcessing.Application.Products.Commands.DeleteProduct;
using OrderProcessing.WebApi.Extensions;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    /// <summary>
    /// Creates a new product in the system.
    /// </summary>
    /// <param name="command">The product details</param>
    /// <returns>The ID of the newly created product</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        // إرسال الـ Command لـ MediatR، والـ Handler هيقوم بالواجب ويرجع الـ Id
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
