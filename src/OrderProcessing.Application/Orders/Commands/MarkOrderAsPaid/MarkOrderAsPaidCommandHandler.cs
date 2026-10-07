using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandHandler(IApplicationDbContext context)
    : IRequestHandler<MarkOrderAsPaidCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            return Result.Failure(OrderErrors.NotFound(request.OrderId));

        order.Status = OrderStatus.Confirmed;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}