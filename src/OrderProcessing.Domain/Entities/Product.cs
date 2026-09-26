using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public List<string> Pics { get; set; } = new List<string>();

    public ICollection<Tag> Tags { get; set; } = [];
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; } = string.Empty;
    //public string SellerName { get; set; }
    public bool IsActive { get; set; }
}

//  id , name , description , pics[] , price , Stock ,UserId , sellerName , IsActive )