using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Queries.GetAllProducts;

public record GetAllProductsQuery(
    string? SearchTerm,
    List<int>? TagIds,
    decimal? MinPrice,
    decimal? MaxPrice
) : IRequest<Result<List<ProductSummaryResponse>>>;