using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderProcessing.Application.Carts.Commands.AddToCart;

public record AddToCartCommand(
    string UserId,
    int ProductId,
    int Quantity
) : IRequest<Result>;