
using OrderProcessing.Application.Carts.Commands.AddToCart;
using OrderProcessing.Application.Carts.Commands.DeleteCartItem;
using OrderProcessing.Application.Carts.Commands.UpdateCartItem;
using OrderProcessing.Application.Carts.Queries.GetAllCartItems;
using OrderProcessing.WebApi.Contracts.Carts;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost("")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request, CancellationToken cancellationToken)
    {
        var userId = "user-123"; // Replace with actual
        var command = new AddToCartCommand(userId, request.ProductId, request.Quantity);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut("{cartId}/items/{productId}")]
    public async Task<IActionResult> UpdateCartItem([FromRoute]int cartId, [FromRoute] int productId, [FromBody] UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        var userId = "user-123"; // Replace with actual

        var command = new UpdateCartItemCommand(userId, cartId, productId, request.Quantity);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpDelete("{cartId}/items/{productId}")]
    public async Task<IActionResult> DeleteCartItem([FromRoute]int cartId, [FromRoute] int productId, CancellationToken cancellationToken)
    {
        var userId = "user-123"; // Replace with actual

        var command = new DeleteCartItemCommand(userId, cartId, productId);

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpGet("my-cart")]
    public async Task<IActionResult> GetCart( CancellationToken cancellationToken)
    {
        var userId = "user-123"; // Replace with actual

        var query = new GetCartQuery(userId);

        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
