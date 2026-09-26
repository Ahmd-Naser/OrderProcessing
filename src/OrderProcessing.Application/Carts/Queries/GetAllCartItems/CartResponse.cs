using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetAllCartItems;

public record CartResponse(
    IEnumerable<CartItemResponse> Items,
    decimal TotalAmount
);