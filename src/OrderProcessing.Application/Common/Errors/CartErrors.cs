
namespace OrderProcessing.Application.Common.Errors;

public static class CartErrors
{
    public static Error NotFound(int cartId) =>
        new Error("Cart.NotFound", $"Cart with ID {cartId} was not found.", (int)HttpStatusCodes.NotFound);

    public static Error NotFoundCartItem(int productId) =>
        new Error("Cart.NotFound", $"Product ID {productId} was not found In the cart.", (int)HttpStatusCodes.NotFound);

    public static Error Forbidden() =>
        new Error("Cart.Forbidden", "You do not have permission to modify or access this cart.", (int)HttpStatusCodes.Forbidden );
}