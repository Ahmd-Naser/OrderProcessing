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

        var command = new PlaceOrderCommand(userId!);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
