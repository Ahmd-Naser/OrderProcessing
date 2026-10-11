using MediatR;
using OrderProcessing.Application.Common.Models;
using System.Text.Json.Serialization;

namespace OrderProcessing.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    List<string> Pics,
    [property: JsonIgnore] string UserId
) : IRequest<Result<int> >; // هيرجع الـ Id بتاع المنتج الجديد بعد ما يتكريت