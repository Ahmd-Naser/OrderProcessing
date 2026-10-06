using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;

namespace OrderProcessing.Application.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(IApplicationDbContext context ) : IRequestHandler<PlaceOrderCommand, Result<int>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<int>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        

        var cart = await _context.Carts
        .Include(c => c.CartItems)
        .ThenInclude(ci => ci.Product)
        .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if(cart is null || !cart.CartItems.Any())
            return Result.Failure<int>( CartErrors.EmptyCart());
            

        var order = new Order
        {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        await _context.Orders.AddAsync(order, cancellationToken);

        foreach (var item in cart.CartItems)
        {
            if (!item.Product.IsActive || item.Product.Stock < item.Quantity) 
                return Result.Failure<int>(CartErrors.ProductNotAvailable());


            await _context.Products
                .Where(p => p.Id == item.ProductId)
                .ExecuteUpdateAsync(p => p.SetProperty(pr => pr.Stock, pr => pr.Stock - item.Quantity), cancellationToken);

        }


        await _context.CartItems
            .Where(ci => ci.CartId == cart.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);


        return Result.Success(order.Id);
     

    }
}
