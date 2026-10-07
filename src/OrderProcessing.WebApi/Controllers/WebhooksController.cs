using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderProcessing.Application.Orders.Commands.MarkOrderAsPaid;
using Stripe;

namespace OrderProcessing.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebhooksController(ISender sender, IConfiguration configuration) : ControllerBase
{
    private readonly ISender _mediator = sender;
    private readonly IConfiguration _configuration = configuration;

    [HttpPost]
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

            return Ok();
        }
        catch (StripeException e)
        {
            // لو التوقيع فشل أو الـ JSON تالف
            return BadRequest(e.Message);
        }
    }
}