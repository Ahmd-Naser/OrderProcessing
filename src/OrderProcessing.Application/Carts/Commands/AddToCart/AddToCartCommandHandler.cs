using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;


namespace OrderProcessing.Application.Carts.Commands.AddToCart;

public class AddToCartCommandHandler(IApplicationDbContext context) : IRequestHandler<AddToCartCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        if(await _context.Carts.Include(c => c.CartItems).SingleOrDefaultAsync(c => c.UserId == request.UserId , cancellationToken) is not { } cart)
        {
            cart = new Cart
            {
                UserId = request.UserId,
            };

            await _context.Carts.AddAsync(cart, cancellationToken);
        }

        if(!await _context.Products.AnyAsync(p => p.Id == request.ProductId && p.IsActive , cancellationToken) )
            return Result.Failure(ProductErrors.NotFound(request.ProductId));

        var cartItem = cart.CartItems.SingleOrDefault(ci => ci.ProductId == request.ProductId);

        if (cartItem is not null)
        {
            cartItem.Quantity += request.Quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
