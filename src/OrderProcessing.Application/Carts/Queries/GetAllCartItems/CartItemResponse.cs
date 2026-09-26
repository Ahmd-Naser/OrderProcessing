namespace OrderProcessing.Application.Carts.Queries.GetAllCartItems;

public record CartItemResponse(
    int ProductId,
    string ProductName,
    decimal UnitPrice,  // سعر القطعة الواحدة
    int Quantity,
    decimal TotalPrice
);