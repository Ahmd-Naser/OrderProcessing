using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponse> >;