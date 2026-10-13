using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderProcessing.Application.Orders.Commands.MarkOrderAsPaid;
using Stripe;
using OrderProcessing.Application.Orders.Commands.CancelOrder;

namespace OrderProcessing.WebApi.Controllers;

/// <summary>
/// Handles incoming webhooks from external payment providers (e.g., Stripe).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class WebhooksController(ISender sender, IConfiguration configuration) : ControllerBase
{
    private readonly ISender _mediator = sender;
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Endpoint for receiving Stripe webhook events.
    /// </summary>
    /// <remarks>
    /// This endpoint is called automatically by Stripe when certain events occur (e.g., successful payment or session expiration).
    /// It verifies the cryptographic signature to ensure the request is genuinely from Stripe, then dispatches the appropriate command to update the order status.
    /// 
    /// Handled Events:
    /// - `checkout.session.completed`: Marks the corresponding order as Paid.
    /// - `checkout.session.expired`: Cancels the corresponding order.
    /// </remarks>
    /// <returns>Returns 200 OK to acknowledge receipt of the event, or 400 Bad Request if signature verification fails.</returns>
    /// <response code="200">The webhook event was successfully received and processed.</response>
    /// <response code="400">The request is invalid, missing a signature, or failed cryptographic verification.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            // 1. قراءة التوقيع الأمني القادم من سترايب في الـ Headers
            var stripeSignature = Request.Headers["Stripe-Signature"];

            // 2. مفتاح الـ Webhook السرّي (هنجيبه من لوحة تحكم سترايب أو ملف الـ appsettings)
            var webhookSecret = _configuration["Stripe:WebhookSecret"];

            // التحقق من أن الحدث قادم فعلاً من Stripe بطريقة مشفرة وآمنة
            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

            // 3. التحقق إذا كان الحدث هو نجاح عملية الدفع
            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                if (session != null)
                {
                    if (int.TryParse(session.ClientReferenceId, out var orderId))
                    {
                        var command = new MarkOrderAsPaidCommand(orderId);
                        await _mediator.Send(command);
                    }
                }
            }

            else if (stripeEvent.Type == "checkout.session.expired")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                if(session != null)
                {
                    if (int.TryParse(session.ClientReferenceId, out var orderId))
                    {
                        var command = new CancelOrderCommand(orderId);
                        await _mediator.Send(command);
                    }
                }
            }

            return Ok();
        }
        catch (StripeException e)
        {
            // لو التوقيع فشل أو الـ JSON تالف
            return BadRequest(e.Message);
        }
    }

    
}