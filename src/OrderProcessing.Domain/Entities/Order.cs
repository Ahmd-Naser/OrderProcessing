
using OrderProcessing.Domain.Enums;

namespace OrderProcessing.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<OrderItem> OrderItems { get; set; } = [];
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }

    public Payment? Payment { get; set; }
}

