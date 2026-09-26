using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Tags.Queries.GetTagById;

internal class GetTagByIdHandler(IApplicationDbContext context) : IRequestHandler<GetTagByIdQuery, Result<TagResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<TagResponse>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {

        if(await _context.Tags.AsNoTracking().FirstOrDefaultAsync(t => t.Id == request.Id , cancellationToken) is not { } tag)
            return Result.Failure<TagResponse>(TagErrors.NotFound(request.Id));

        var response = new TagResponse(tag.Id, tag.Name);

        return Result.Success(response);
    }
}
