using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Orders.Commands.PlaceOrder;
using System.Reflection;
using System.Security.Claims;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(ISender sender , IPaymentService paymentService) : ControllerBase
{
    private readonly ISender _mediator = sender;
    private readonly IPaymentService _paymentService = paymentService;

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        // هنجيب الـ UserId من الـ Token (Claims)
        var userId = "user-123";

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
}
