using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Tags.Queries.GetTagById;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Queries.GetAllTags;

public class GetAllTagsHandler(IApplicationDbContext context) : IRequestHandler<GetAllTagsQuery, Result<List<TagResponse>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<List<TagResponse>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tags.AsQueryable();


        if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(t => t.Name.Contains(request.SearchTerm));


        var tagsResponse = await query.Select(t => new TagResponse(t.Id, t.Name)).ToListAsync(cancellationToken);

        return Result.Success(tagsResponse);
    }
}
