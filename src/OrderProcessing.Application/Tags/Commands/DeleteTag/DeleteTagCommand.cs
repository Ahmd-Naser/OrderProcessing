using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.DeleteTag;

public record DeleteTagCommand (int Id) : IRequest<Result>;