
namespace OrderProcessing.WebApi.Contracts.Carts;

public record AddToCartRequest(
    int ProductId,
    int Quantity
);