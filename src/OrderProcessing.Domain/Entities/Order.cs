
using OrderProcessing.Domain.Enums;

namespace OrderProcessing.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; } 
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public Payment? Payment { get; set; }
}

