using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.CreateTag;

public class CreateTagCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTagCommand, Result<int>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<int>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Tags.AnyAsync(t => t.Name == request.Name, cancellationToken) )
            return Result.Failure<int>(TagErrors.AlreadyExists(request.Name));
        var tag = new Tag
        {
            Name = request.Name
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(tag.Id);
    }
}
