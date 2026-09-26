using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Carts.Queries.GetAllCartItems;

public class GetCartHandler(IApplicationDbContext context) : IRequestHandler<GetCartQuery, Result<CartResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        // 1. Projection لـ Anonymous Type (EF Core بتعشقه ومبيضربش خالص)
        var cartData = await _context.Carts
            .Where(c => c.UserId == request.UserId)
            .Select(c => new
            {
                Items = c.CartItems.Select(ci => new
                {
                    ci.ProductId,
                    ci.Product.Name,
                    ci.Product.Price,
                    ci.Quantity,
                    TotalPrice = ci.Product.Price * ci.Quantity
                }),
                TotalAmount = c.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            })
            .SingleOrDefaultAsync(cancellationToken);

        // 2. معالجة حالة إن العربة فارغة أو مش موجودة
        if (cartData is null)
        {
            // نرجع عربة فاضية بصفر، لأن اليوزر ممكن ميكونش ضاف حاجة لسه
            return Result.Success(new CartResponse(Enumerable.Empty<CartItemResponse>(), 0));
        }

        // 3. Mapping للـ Records بتاعتنا في الميموري (سريع جداً ومفيش أي أثر على الـ Performance)
        var response = new CartResponse(
            cartData.Items.Select(i => new CartItemResponse(
                i.ProductId,
                i.Name,
                i.Price,
                i.Quantity,
                i.TotalPrice
            )),
            cartData.TotalAmount
        );

        return Result.Success(response);
    }
}
