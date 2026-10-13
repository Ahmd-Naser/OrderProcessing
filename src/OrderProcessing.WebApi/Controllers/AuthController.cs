using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OrderProcessing.Application.Auth.Commands.RegisterCustomer;
using OrderProcessing.Application.Auth.Commands.RegisterVendor;
using OrderProcessing.Application.Auth.Common;

namespace OrderProcessing.WebApi.Controllers;

/// <summary>
/// Handles user authentication and registration operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender mediator) : ControllerBase
{
    private readonly ISender _mediator = mediator;

    /// <summary>
    /// Registers a new customer account.
    /// </summary>
    /// <remarks>
    /// Customers are the end-users who can browse products, add them to carts, and place orders.
    /// Upon successful registration, this endpoint returns the authentication details including a JWT token.
    /// </remarks>
    /// <param name="command">The customer's registration details (e.g., Name, Email, Password).</param>
    /// <returns>Authentication result containing the user details and JWT token.</returns>
    /// <response code="200">The customer was successfully registered and authenticated.</response>
    /// <response code="400">If the provided data is invalid or the email is already in use.</response>
    [HttpPost("register-customer")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Registers a new vendor account.
    /// </summary>
    /// <remarks>
    /// Vendors are the merchants who can manage products, stock, and view their sales.
    /// Upon successful registration, this endpoint returns the authentication details including a JWT token with Vendor claims.
    /// </remarks>
    /// <param name="command">The vendor's registration details (e.g., Store Name, Email, Password).</param>
    /// <returns>Authentication result containing the vendor details and JWT token.</returns>
    /// <response code="200">The vendor was successfully registered and authenticated.</response>
    /// <response code="400">If the provided data is invalid or the email is already in use.</response>
    [HttpPost("register-vendor")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    /// <summary>
    /// Authenticates an existing user (Customer or Vendor) and issues a JWT token.
    /// </summary>
    /// <param name="command">The user's login credentials (Email and Password).</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Authentication result containing the user details and a new JWT token.</returns>
    /// <response code="200">The user was successfully authenticated.</response>
    /// <response code="400">If the credentials format is invalid.</response>
    /// <response code="401">If the email or password is incorrect (Unauthorized).</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> Login([FromBody] LoginCommand command , CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
