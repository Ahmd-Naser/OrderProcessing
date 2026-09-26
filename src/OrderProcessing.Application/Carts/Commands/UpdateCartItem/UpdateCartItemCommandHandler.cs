using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Carts.Commands.UpdateCartItem;

public class UpdateCartItemCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateCartItemCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Result> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var effectedRows = await _context.CartItems
            .Where(ci => ci.Cart.UserId == request.UserId && ci.ProductId == request.ProductId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ci => ci.Quantity, request.Quantity),
                cancellationToken);

        if (effectedRows == 0)
        {
            // لو مفيش صفوف اتعدلت، يبقى إما الـ ProductId مش في العربة، أو العربة مش بتاعت اليوزر ده.
            return Result.Failure(CartErrors.NotFoundCartItem(request.ProductId));
        }

        return Result.Success();
    }
}
