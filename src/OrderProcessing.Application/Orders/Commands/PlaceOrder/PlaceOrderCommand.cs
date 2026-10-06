using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Orders.Commands.PlaceOrder;

public record PlaceOrderCommand(
    string UserId,
    Guid IdempotencyKey
) : IRequest<Result<int> > , IIdempotentCommand;