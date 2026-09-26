using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderProcessing.Application.Products.Commands.AssignTagsToProduct;

public record AssignTagsToProductCommand(
    [property: JsonIgnore] int ProductId,
    List<int> TagIds
) : IRequest<Result>;
