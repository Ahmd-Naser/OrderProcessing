using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetAllCartItems;

public record GetCartQuery(
    string UserId
) : IRequest<Result<CartResponse>>;