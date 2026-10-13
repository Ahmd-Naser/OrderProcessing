using Microsoft.AspNetCore.Authorization;
using OrderProcessing.Application.Products.Commands.AssignTagsToProduct;
using OrderProcessing.Application.Products.Commands.CreateProduct;
using OrderProcessing.Application.Products.Commands.DeleteProduct;
using OrderProcessing.Application.Products.Commands.UpdateIsActiveProduct;
using OrderProcessing.Application.Products.Commands.UpdateProduct;
using OrderProcessing.Application.Products.Queries.GetAllProducts;
using OrderProcessing.Application.Products.Queries.GetProductById;
using OrderProcessing.Domain.Consts;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    /// <summary>
    /// Creates a new product in the system.
    /// </summary>
    /// <remarks>
    /// The user must have the Vendor role to be able to add products.
    /// </remarks>
    /// <param name="command">The product details (Name, Price, Stock, etc.).</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns the ID of the newly created product.</returns>
    /// <response code="201">The product was successfully created.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user does not have the Vendor role.</response>
    [HttpPost]
    [Authorize(DefaultRoles.Vendor)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]

    [Authorize(DefaultRoles.Vendor)]
    public async Task<ActionResult<int>> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var updatedCommand = command with { UserId = userId! };
        var result = await _mediator.Send(updatedCommand , cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = result.Value }, result.Value);
    }

    /// <summary>
    /// Updates an existing product's details.
    /// </summary>
    /// <remarks>
    /// Only the owner of the product (Vendor) can modify its details.
    /// </remarks>
    /// <param name="id">The ID of the product to update.</param>
    /// <param name="command">The updated product data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 204 No Content upon success.</returns>
    [HttpPut("{id}")]
    [Authorize(DefaultRoles.Vendor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var updatedCommand = command with { Id = id , UserId = userId!};
        var result = await _mediator.Send(updatedCommand, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    /// <summary>
    /// Permanently deletes a product from the system.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 204 No Content upon success.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new DeleteProductCommand(id, userId!);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    /// <summary>
    /// Retrieves a specific product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The product details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    /// <summary>
    /// Retrieves a list of products with optional search and filtering.
    /// </summary>
    /// <param name="searchTerm">A search term to filter product names.</param>
    /// <param name="tagIds">A list of Tag IDs to filter products by.</param>
    /// <param name="minPrice">The minimum price filter.</param>
    /// <param name="maxPrice">The maximum price filter.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A list of products matching the search criteria.</returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Toggles the active status of a product (Active / Inactive).
    /// </summary>
    /// <remarks>
    /// Reverses the current status (if active, makes it inactive, and vice versa). Only the product owner can execute this.
    /// </remarks>
    /// <param name="id">The ID of the product.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 204 No Content upon success.</returns>
    [HttpPut("{id}/is-active-toggle")]
    [Authorize(DefaultRoles.Vendor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ToggleIsActive([FromRoute] int id, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var command = new UpdateIsActiveProductCommand(id, userId!);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    /// <summary>
    /// Assigns specific tags to a product.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <param name="command">A list of Tag IDs to assign.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 204 No Content upon success.</returns>
    [HttpPut("{id}/tags")]
    [Authorize(DefaultRoles.Vendor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AssignTags([FromRoute] int id, [FromBody] AssignTagsToProductCommand command, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var updatedCommand = command with { ProductId = id, UserId = userId! };
        var result = await _mediator.Send(updatedCommand, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
