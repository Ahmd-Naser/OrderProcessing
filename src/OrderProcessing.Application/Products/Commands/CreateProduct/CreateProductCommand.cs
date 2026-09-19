using MediatR;

namespace OrderProcessing.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string UserId
) : IRequest<int>; // هيرجع الـ Id بتاع المنتج الجديد بعد ما يتكريت