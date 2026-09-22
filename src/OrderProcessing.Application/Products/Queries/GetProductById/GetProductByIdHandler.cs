
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler(IApplicationDbContext context) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if(product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound(request.Id));

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.Pics 
        );

        return Result.Success(response);
    }
}
