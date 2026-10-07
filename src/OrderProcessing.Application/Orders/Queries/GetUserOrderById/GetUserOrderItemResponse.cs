using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Queries.GetUserOrderById;

public record GetUserOrderItemResponse(
    int Id,
    string Name,
    decimal Price,
    int Quantity
);