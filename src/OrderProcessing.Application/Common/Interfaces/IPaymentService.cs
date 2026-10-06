using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<Result<string>> CreateCheckoutSessionAsync(int orderId, CancellationToken cancellationToken);
}