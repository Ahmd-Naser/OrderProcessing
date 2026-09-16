using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Enums;

public enum OrderStatus
{
    Pending = 1,
    Confirmed = 2,
    Processing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6,
    PaymentFailed = 7
}
