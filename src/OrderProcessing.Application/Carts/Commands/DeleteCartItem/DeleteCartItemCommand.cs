

namespace OrderProcessing.Application.Carts.Commands.DeleteCartItem;

public record DeleteCartItemCommand(
    string UserId,
    int CartId,
    int ProductId
) : IRequest<Result>;