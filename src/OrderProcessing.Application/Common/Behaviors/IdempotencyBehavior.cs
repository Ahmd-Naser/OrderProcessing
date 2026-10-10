using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse>(IApplicationDbContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommand
    where TResponse : Result // لضمان قدرتنا على إرجاع Result.Failure
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. الفحص
        if (await _context.IdempotencyKeys.AnyAsync(k => k.Key == request.IdempotencyKey, cancellationToken))
        {
            // نرجع الفشل فوراً بدون تنفيذ الـ Handler
            return (TResponse)Result.Failure(OrderErrors.DuplicateRequest());
        }

        // 2. تسجيل الـ Key (سيتم حفظه فعلياً مع حفظ الأوردر في الـ Handler)
        await _context.IdempotencyKeys.AddAsync(new IdempotencyKey
        {
            Key = request.IdempotencyKey,
            RequestName = typeof(TRequest).Name,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        // 3. تمرير الريكويست للـ Handler
        return await next();
    }
}