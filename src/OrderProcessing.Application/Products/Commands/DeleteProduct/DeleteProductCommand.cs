
namespace OrderProcessing.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(int Id) : IRequest<Result>; // هيرجع Result indicating success or failure