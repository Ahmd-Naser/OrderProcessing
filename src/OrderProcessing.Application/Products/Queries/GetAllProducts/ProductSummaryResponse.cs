using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Queries.GetAllProducts;

public record ProductSummaryResponse(
    int Id,
    string Name,
    decimal Price,
    int Stock,
    string? ThumbnailUrl // بنرجع String واحد لصورة واحدة مش List
);