using OrderProcessing.Application.Carts.Queries.GetAllCartItems;
using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetCheckoutPreview;

public class GetCheckoutPreviewHandler( IApplicationDbContext context ) : IRequestHandler<GetCheckoutPreviewQuery, Result<GetCheckoutPreviewResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<GetCheckoutPreviewResponse>> Handle(GetCheckoutPreviewQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if(cart is null || !cart.CartItems.Any())
            return Result.Failure<GetCheckoutPreviewResponse>(CartErrors.EmptyCart());

        var items = new List<CartItemResponse>();
        var totalPrice = 0m;

        foreach (var item in cart!.CartItems)
        {
           
            if(item.Product is null || !item.Product.IsActive || (item.Product.Stock < item.Quantity))
                return Result.Failure<GetCheckoutPreviewResponse>(CartErrors.ProductNotAvailable());

            totalPrice += item.Product.Price * item.Quantity;
            items.Add(new CartItemResponse
            (
                item.ProductId,
                item.Product.Name,
                item.Product.Price,
                item.Quantity,
                item.Product.Price * item.Quantity
            ));


        }

        return Result.Success(new GetCheckoutPreviewResponse(items, totalPrice));
    }
}
