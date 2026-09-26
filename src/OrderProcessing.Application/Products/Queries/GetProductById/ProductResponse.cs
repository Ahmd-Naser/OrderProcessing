using OrderProcessing.Application.Tags.Queries.GetTagById;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Queries.GetProductById;

public record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Stock,
    List<TagResponse> Tags,
    List<string> Pics
);