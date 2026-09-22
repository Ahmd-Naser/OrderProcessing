using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OrderProcessing.Application.Products.Commands.DeleteProduct;

internal class DeleteProductCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await _context.Products
            .Where(p => p.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (rowsAffected == 0)
            return Result.Failure(ProductErrors.NotFound(request.Id) );

        return Result.Success();
    }
}
