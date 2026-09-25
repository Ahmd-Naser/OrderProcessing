using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Queries.GetTagById;

public record TagResponse(
    int Id,
    string Name
);
