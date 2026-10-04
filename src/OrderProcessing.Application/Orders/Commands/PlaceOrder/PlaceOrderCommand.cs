using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Commands.PlaceOrder;

public record PlaceOrderCommand(
    string UserId
) : IRequest<Result>;