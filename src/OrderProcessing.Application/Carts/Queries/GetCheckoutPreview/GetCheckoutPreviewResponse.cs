using OrderProcessing.Application.Carts.Queries.GetAllCartItems;

namespace OrderProcessing.Application.Carts.Queries.GetCheckoutPreview;

public record GetCheckoutPreviewResponse(
    IEnumerable<CartItemResponse> Items,
    decimal TotalPrice
);