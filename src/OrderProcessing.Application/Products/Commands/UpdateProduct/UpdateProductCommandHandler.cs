using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessing.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync(request.Id, cancellationToken);

        if (product == null)
            return Result.Failure(ProductErrors.ProductNotFound(request.Id));

        // Update the product properties
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.Pics = request.Pics;
        product.UserId = request.UserId;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}   

  