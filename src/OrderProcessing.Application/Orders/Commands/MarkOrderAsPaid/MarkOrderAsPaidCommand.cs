using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Commands.MarkOrderAsPaid;

public record MarkOrderAsPaidCommand(int OrderId) : IRequest<Result>, ITransactionalCommand;