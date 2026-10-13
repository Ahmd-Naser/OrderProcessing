
using Microsoft.AspNetCore.Authorization;
using OrderProcessing.Application.Carts.Commands.AddToCart;
using OrderProcessing.Application.Carts.Commands.DeleteCartItem;
using OrderProcessing.Application.Carts.Commands.UpdateCartItem;
using OrderProcessing.Application.Carts.Queries.GetAllCartItems;
using OrderProcessing.Application.Carts.Queries.GetCheckoutPreview;
using OrderProcessing.Domain.Consts;
using OrderProcessing.WebApi.Contracts.Carts;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(DefaultRoles.Customer)]
public class CartsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    /// <summary>
    /// Adds a product to the authenticated user's cart.
    /// </summary>
    /// <remarks>
    /// If the product already exists in the cart, its quantity will be increased.
    /// The user must be authenticated with the Customer role.
    /// </remarks>
    /// <param name="request">The product ID and the quantity to add.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 200 OK upon successful addition.</returns>
    /// <response code="200">The product was successfully added to the cart.</response>
    /// <response code="400">If the provided product ID is invalid or quantity is zero/negative.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user does not have the Customer role.</response>
    /// <response code="404">If the product does not exist or is inactive.</response>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId(); 
        var command = new AddToCartCommand(userId!, request.ProductId, request.Quantity);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Updates the quantity of a specific product in the user's cart.
    /// </summary>
    /// <param name="cartId">The ID of the cart.</param>
    /// <param name="productId">The ID of the product to update.</param>
    /// <param name="request">The new quantity for the product.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 200 OK upon successful update.</returns>
    [HttpPut("{cartId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> UpdateCartItem([FromRoute]int cartId, [FromRoute] int productId, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new UpdateCartItemCommand(userId!, cartId, productId, request.Quantity);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Removes a specific product entirely from the user's cart.
    /// </summary>
    /// <param name="cartId">The ID of the cart.</param>
    /// <param name="productId">The ID of the product to remove.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns 200 OK upon successful deletion.</returns>
    [HttpDelete("{cartId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> DeleteCartItem([FromRoute]int cartId, [FromRoute] int productId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var command = new DeleteCartItemCommand(userId!, cartId, productId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    /// <summary>
    /// Retrieves the current active cart for the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The user's cart details including all items and total price.</returns>
    [HttpGet("my-cart")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetCart( CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var query = new GetCartQuery(userId!);

        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Generates a checkout preview for the user's cart.
    /// </summary>
    /// <remarks>
    /// Validates all items in the cart against current stock levels and calculates the final total amount before placing the actual order.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A preview containing validated items, stock status, and final calculations.</returns>
    [HttpGet("checkout-preview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetCheckoutPreview(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var query = new GetCheckoutPreviewQuery(userId!);

        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
