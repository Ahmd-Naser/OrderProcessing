using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Interfaces;

namespace OrderProcessing.Application.Tags.Queries.GetTagById;

internal class GetTagByIdHandler(IApplicationDbContext context) : IRequestHandler<GetTagByIdQuery, Result<TagResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<TagResponse>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags.FindAsync(request.Id, cancellationToken);

        if (tag == null)
            return Result.Failure<TagResponse>(TagErrors.NotFound(request.Id));

        var response = new TagResponse(tag.Id, tag.Name);

        return Result.Success(response);
    }
}
