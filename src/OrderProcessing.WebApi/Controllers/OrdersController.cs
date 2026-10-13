using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Orders.Commands.PlaceOrder;
using OrderProcessing.Application.Orders.Queries.GetUserOrderById;
using OrderProcessing.Application.Orders.Queries.GetUserOrders;
using OrderProcessing.Domain.Consts;
using System.Reflection;
using System.Security.Claims;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(ISender sender , IPaymentService paymentService) : ControllerBase
{
    private readonly ISender _mediator = sender;
    private readonly IPaymentService _paymentService = paymentService;

    /// <summary>
    /// Places a new order based on the user's current cart and initiates the payment process.
    /// </summary>
    /// <remarks>
    /// **Idempotency Requirement:**
    /// You must include an `X-Idempotency-Key` header with a valid GUID in the request. 
    /// This prevents duplicate orders from being created if the client retries the request due to a network timeout.
    /// 
    /// If successful, this endpoint returns a Stripe Checkout Session URL. The client should redirect the user to this URL to complete payment.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>An object containing the Stripe Checkout URL.</returns>
    /// <response code="200">Order successfully created and payment session initiated. Returns the Stripe URL.</response>
    /// <response code="400">If the cart is empty, products are out of stock, or the Idempotency-Key is missing/invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    [Authorize(DefaultRoles.Customer)] // Only customers can place orders
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        if (!Request.Headers.TryGetValue("X-Idempotency-Key", out var headerValue) ||
            !Guid.TryParse(headerValue, out var idempotencyKey))
        {
            return BadRequest("Missing or invalid X-Idempotency-Key header. Must be a valid GUID.");
        }

        var command = new PlaceOrderCommand(userId!, idempotencyKey);
        var orderResult = await _mediator.Send(command, cancellationToken);

        if (!orderResult.IsSuccess)
            return orderResult.ToProblem();

        var result = await _paymentService.CreateCheckoutSessionAsync(orderResult.Value, cancellationToken);

        return result.IsSuccess ? Ok(new {url = result.Value }) : result.ToProblem();
    }

    /// <summary>
    /// Retrieves a history of all orders placed by the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A list of the user's orders, including their statuses and total amounts.</returns>
    /// <response code="200">Returns the list of orders.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("my-orders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserOrders( CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var query = new GetUserOrdersQuery(userId!);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Retrieves the detailed information of a specific order belonging to the user.
    /// </summary>
    /// <param name="orderId">The ID of the order.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The detailed order information, including items and payment status.</returns>
    /// <response code="200">Returns the specific order details.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="403">If the user attempts to access an order they do not own.</response>
    /// <response code="404">If the order is not found.</response>
    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetUserOrderById(int orderId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var query = new GetUserOrderByIdQuery(userId!, orderId);
        var result = await _mediator.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
