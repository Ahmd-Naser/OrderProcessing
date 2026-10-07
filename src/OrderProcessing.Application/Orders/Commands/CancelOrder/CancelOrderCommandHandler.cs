using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(IApplicationDbContext context) : IRequestHandler<CancelOrderCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if(order == null)
            return Result.Failure(OrderErrors.NotFound(request.OrderId));

        foreach(var i in order.OrderItems)
        {
            i.Product.Stock += i.Quantity;

        }

        order.Status = OrderStatus.Cancelled;

        await _context.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }
}
