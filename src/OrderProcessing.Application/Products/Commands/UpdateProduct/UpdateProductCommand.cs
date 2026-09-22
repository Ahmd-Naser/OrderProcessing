
using System.Text.Json.Serialization;

namespace OrderProcessing.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    [property: JsonIgnore]int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    List<string> Pics,
    string UserId
) : IRequest<Result>;