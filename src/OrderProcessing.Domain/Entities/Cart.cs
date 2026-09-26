namespace OrderProcessing.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public ICollection<CartItem> CartItems { get; set; } = [];
}
// cart ( id , userId , ProductId, Quantity )