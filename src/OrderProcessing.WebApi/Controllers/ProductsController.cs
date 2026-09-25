using OrderProcessing.Application.Products.Commands.CreateProduct;
using OrderProcessing.Application.Products.Commands.DeleteProduct;
using OrderProcessing.Application.Products.Commands.UpdateIsActiveProduct;
using OrderProcessing.Application.Products.Commands.UpdateProduct;
using OrderProcessing.Application.Products.Queries.GetAllProducts;
using OrderProcessing.Application.Products.Queries.GetProductById;

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
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The ID of the newly created product</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var updatedCommand = command with { Id = id };
        var result = await _mediator.Send(updatedCommand, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm , [FromQuery] List<int>? tagIds, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, CancellationToken cancellationToken)
    {
        var query = new GetAllProductsQuery 
        (
            searchTerm,
            tagIds,
            minPrice,
            maxPrice
        );

        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id}/is-active-toggle")]
    public async Task<IActionResult> ToggleIsActive([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new UpdateIsActiveProductCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
