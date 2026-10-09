using Microsoft.AspNetCore.Identity;

namespace OrderProcessing.Persistence.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? StoreName { get; set; }
}
