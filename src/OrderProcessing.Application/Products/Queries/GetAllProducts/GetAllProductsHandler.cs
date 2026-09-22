using OrderProcessing.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace OrderProcessing.Application.Products.Queries.GetAllProducts;

public class GetAllProductsHandler(IApplicationDbContext context) : IRequestHandler<GetAllProductsQuery, Result<List<ProductSummaryResponse>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<List<ProductSummaryResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsNoTracking();

        if(!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(p=> p.Name.Contains(request.SearchTerm));

        if(request.TagIds != null && request.TagIds.Any() )
            query = query.Where(p => p.Tags.Any(t => request.TagIds.Contains(t.Id) ));

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if(request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        var dbResult = await query
             .Select(p => new
             {
                 p.Id,
                 p.Name,
                 p.Price,
                 p.Stock,
                 p.Pics // بنجيب عمود الـ Pics زي ما هو كـ JSON
             })
             .ToListAsync(cancellationToken);

        // 3. Mapping للـ Record بتاعنا (بيحصل في الميموري بسرعة الصاروخ)
        var responses = dbResult
            .Select(p => new ProductSummaryResponse(
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.Pics?.FirstOrDefault() // هنا بقى C# هي اللي بتنفذ الـ FirstOrDefault براحتها بدون مشاكل SQL
            ))
            .ToList();

        return Result.Success(responses);
    }
}
