
namespace OrderProcessing.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(
    int Id ,
    string UserId
) : IRequest<Result>; // هيرجع Result indicating success or failure