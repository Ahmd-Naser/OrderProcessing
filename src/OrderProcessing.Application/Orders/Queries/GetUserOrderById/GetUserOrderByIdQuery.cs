using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrderById;

public record GetUserOrderByIdQuery(
    string UserId,
    int OrderId
) : IRequest<Result<GetUserOrderByIdResponse>>;