using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Tags.Commands.DeleteTag;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Tag Id must be greater than zero.");
    }
}
