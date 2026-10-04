
namespace OrderProcessing.Application.Common.Errors;

public static class CartErrors
{
    public static Error NotFound(int cartId) =>
        new Error("Cart.NotFound", $"Cart with ID {cartId} was not found.", (int)HttpStatusCodes.NotFound);

    public static Error NotFoundCartItem(int productId) =>
        new Error("Cart.NotFound", $"Product ID {productId} was not found In the cart.", (int)HttpStatusCodes.NotFound);

    public static Error Forbidden() =>
        new Error("Cart.Forbidden", "You do not have permission to modify or access this cart.", (int)HttpStatusCodes.Forbidden );

    public static Error ProductNotAvailable() =>
        new Error("Cart.ProductNotAvailable", "One or more products in the cart are not available.", (int)HttpStatusCodes.BadRequest);

    public static Error EmptyCart() =>
        new Error("Cart.Empty", "The cart is empty.", (int)HttpStatusCodes.BadRequest);
}