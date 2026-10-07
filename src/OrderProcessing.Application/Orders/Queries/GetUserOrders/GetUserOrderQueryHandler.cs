using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrders;

public class GetUserOrderQueryHandler(IApplicationDbContext context) : IRequestHandler<GetUserOrdersQuery, Result<IEnumerable<GetUserOrdersResponse>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<IEnumerable<GetUserOrdersResponse>>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetUserOrdersResponse> orders = await _context.Orders
            .Where(o => o.UserId == request.UserId)
            .AsNoTracking()
            .Select(o => new GetUserOrdersResponse(
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                o.Status.ToString()
            ))
            .ToListAsync(cancellationToken);


        return Result.Success(orders) ;
    }
}
