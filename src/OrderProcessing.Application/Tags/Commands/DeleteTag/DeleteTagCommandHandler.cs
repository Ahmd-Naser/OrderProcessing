using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.DeleteTag;

public class DeleteTagCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteTagCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {

        var affectedRows = await _context.Tags
            .Where(t => t.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if(affectedRows == 0)
            return Result.Failure(TagErrors.NotFound(request.Id));


        return Result.Success();
    }
}
