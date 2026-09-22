using MediatR;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Common.Models;

namespace OrderProcessing.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateProductCommand, Result<int> >
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<int> >Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            UserId = request.UserId,
            IsActive = true
        };

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(product.Id); // رجع الـ Id
    }
}