using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Enums;

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}