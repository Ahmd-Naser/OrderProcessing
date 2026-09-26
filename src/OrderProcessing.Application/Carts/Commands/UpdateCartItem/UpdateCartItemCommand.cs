using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Commands.UpdateCartItem;

public record UpdateCartItemCommand(
    string UserId,
    int CartId,
    int ProductId,
    int Quantity
) : IRequest<Result>;