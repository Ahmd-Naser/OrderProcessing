using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrderById;

public record GetUserOrderByIdResponse(
    int OrderId,
    DateTime OrderDate,
    decimal TotalAmount,
    List<GetUserOrderItemResponse> Items,
    string Status
);