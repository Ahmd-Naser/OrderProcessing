using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.CreateTag;

public record CreateTagCommand(string Name) : IRequest<Result<int> >;