using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrderById;

public class GetUserOrderByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetUserOrderByIdQuery, Result<GetUserOrderByIdResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<GetUserOrderByIdResponse>> Handle(GetUserOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Where(o => o.UserId == request.UserId && o.Id == request.OrderId)
            .AsNoTracking()
            .Select(o => new GetUserOrderByIdResponse(
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                o.OrderItems.Select(oi => new GetUserOrderItemResponse(
                    oi.ProductId,
                    oi.Product.Name,
                    oi.UnitPrice,
                    oi.Quantity

                )).ToList(),

                o.Status.ToString()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if(order is null)
            return Result.Failure<GetUserOrderByIdResponse>(OrderErrors.NotFound(request.OrderId));

        return Result.Success(order);
    }
}
