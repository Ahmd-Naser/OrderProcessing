using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrders;

public record GetUserOrdersResponse(
    int OrderId,
    DateTime OrderDate,
    decimal TotalAmount,
    string Status
);