using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.AssignTagsToProduct;

public class AssignTagsToProductCommandHandler(IApplicationDbContext context) : IRequestHandler<AssignTagsToProductCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(AssignTagsToProductCommand request, CancellationToken cancellationToken)
    {
        if(await _context.Products.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound(request.ProductId));

        var newTags = await _context.Tags
            .Where(t => request.TagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

        if(newTags.Count != request.TagIds.Count)
            return Result.Failure(TagErrors.InvalidData());

        

        product.Tags.Clear();

        foreach (var tag in newTags)
        {
            product.Tags.Add(tag);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
