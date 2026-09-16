using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public string UserId { get; set; }
    public bool Status { get; set; }
    public string? TransactionId { get; set; }
}
// (Id, OrderId, UserId, Status :  bool, TransactionId:nullable) 