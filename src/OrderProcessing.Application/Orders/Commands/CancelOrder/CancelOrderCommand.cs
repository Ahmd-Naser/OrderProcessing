using OrderProcessing.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(int OrderId) : IRequest<Result>, ITransactionalCommand;
