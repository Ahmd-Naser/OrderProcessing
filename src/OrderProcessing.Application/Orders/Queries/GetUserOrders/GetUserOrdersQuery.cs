using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrders;

public record GetUserOrdersQuery(string UserId) : IRequest<Result<IEnumerable<GetUserOrdersResponse>>>;