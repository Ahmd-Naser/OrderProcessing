using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Common.Models;
using OrderProcessing.Domain.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Infrastructure.Services;

public class StripePaymentService(IApplicationDbContext context) : IPaymentService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<string>> CreateCheckoutSessionAsync(int orderId, CancellationToken cancellationToken)
    {

        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Result.Failure<string>(OrderErrors.NotFound(orderId));


        // رابط الفرونت إند (مؤقتاً hardcoded وممكن نقرأه من الـ appsettings لاحقاً)
        var domain = "http://localhost:4200";

        var options = new SessionCreateOptions
        {
            //PaymentMethodTypes = ["card"],
            LineItems = [],
            Mode = "payment",
            // Stripe هيبدل {CHECKOUT_SESSION_ID} بالـ ID الفعلي للجلسة
            SuccessUrl = $"{domain}/checkout/success?sessionId={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{domain}/checkout/cancel",
            ClientReferenceId = order.Id.ToString(), // رقم الأوردر بتاعنا لربطه لاحقاً في الـ Webhook
            // يفضل تمرير إيميل العميل هنا لربط الدفع به في لوحة تحكم Stripe
        };

        // تحويل منتجات الأوردر إلى منتجات تقبلها Stripe
        foreach (var item in order.OrderItems)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.UnitPrice * 100), // Stripe تقرأ القيمة بالسنت، لذا نضرب في 100
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Product ID: {item.ProductId}", // يمكنك تمرير اسم المنتج الفعلي إذا كان متوفراً
                    },
                },
                Quantity = item.Quantity,
            });
        }

        var service = new SessionService();
        Session session = await service.CreateAsync(options, cancellationToken: cancellationToken);

        // إرجاع رابط صفحة الدفع المؤمنة
        return Result.Success(session.Url);
    }

    
}