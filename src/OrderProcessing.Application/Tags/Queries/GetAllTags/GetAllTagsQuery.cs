using OrderProcessing.Application.Tags.Queries.GetTagById;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Queries.GetAllTags;

public record GetAllTagsQuery(
    string? SearchTerm
) : IRequest<Result<List<TagResponse>>>;
