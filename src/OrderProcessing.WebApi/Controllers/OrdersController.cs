using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Orders.Commands.PlaceOrder;
using System.Reflection;
using System.Security.Claims;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(ISender sender) : ControllerBase
{
    private readonly ISender _mediator = sender;

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        // هنجيب الـ UserId من الـ Token (Claims)
        var userId = "test_user_id";

        if (!Request.Headers.TryGetValue("X-Idempotency-Key", out var headerValue) ||
            !Guid.TryParse(headerValue, out var idempotencyKey))
        {
            return BadRequest("Missing or invalid X-Idempotency-Key header. Must be a valid GUID.");
        }

        var command = new PlaceOrderCommand(userId!, idempotencyKey);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
