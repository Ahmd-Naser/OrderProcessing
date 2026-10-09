using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.Auth.Commands.RegisterCustomer;
using OrderProcessing.Application.Auth.Commands.RegisterVendor;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    [HttpPost("register-customer")]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("register-vendor")]
    public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
