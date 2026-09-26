using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public string UserId { get; set; } = string.Empty;
    public bool Status { get; set; }
    public string? TransactionId { get; set; }
}
// (Id, OrderId, UserId, Status :  bool, TransactionId:nullable) 