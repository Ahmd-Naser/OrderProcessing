namespace OrderProcessing.Application.Tags.Queries.GetTagById;

public record GetTagByIdQuery(
    int Id
) : IRequest<Result<TagResponse>>;