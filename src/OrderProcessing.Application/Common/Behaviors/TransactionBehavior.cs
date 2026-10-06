using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IApplicationDbContext context)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionalCommand
{
    private readonly IApplicationDbContext _context = context;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // فتح المعاملة قبل وصول الريكويست للـ Handler
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next(); // تشغيل الكوماند (أو المحطة التالية)

            await transaction.CommitAsync(cancellationToken);
            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw; // رمي الخطأ ليتم معالجته في Global Exception Handler
        }
    }
}