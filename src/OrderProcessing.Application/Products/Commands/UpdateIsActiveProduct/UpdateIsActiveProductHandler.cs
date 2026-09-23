using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Products.Commands.UpdateIsActiveProduct;

public class UpdateIsActiveProductHandler(IApplicationDbContext context) : IRequestHandler<UpdateIsActiveProductCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(UpdateIsActiveProductCommand request, CancellationToken cancellationToken)
    {
        
        int rowAffected = await _context.Products
            .Where(p=> p.Id == request.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsActive, p => !p.IsActive), cancellationToken);

        if (rowAffected == 0)
            return Result.Failure(ProductErrors.NotFound(request.Id));

        return Result.Success();
    }
}
