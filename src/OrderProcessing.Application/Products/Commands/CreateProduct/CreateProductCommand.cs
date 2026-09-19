using MediatR;
using OrderProcessing.Application.Common.Models;

namespace OrderProcessing.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    string UserId
) : IRequest<Result<int> >; // هيرجع الـ Id بتاع المنتج الجديد بعد ما يتكريت