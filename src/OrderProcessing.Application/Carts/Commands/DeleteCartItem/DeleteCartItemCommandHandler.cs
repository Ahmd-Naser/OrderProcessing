

using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Carts.Commands.DeleteCartItem;

public class DeleteCartItemCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCartItemCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
    {

        var effectedRows = await _context.CartItems
            .Where(ci => ci.Cart.UserId == request.UserId && ci.ProductId == request.ProductId)
            .ExecuteDeleteAsync(cancellationToken);

        if (effectedRows == 0)
        {
            // لو مفيش صفوف اتمسحت، إما المنتج مش في العربة، أو العربة مش بتاعته
            return Result.Failure(CartErrors.NotFoundCartItem(request.ProductId));
        }


        return Result.Success();

    }
}
