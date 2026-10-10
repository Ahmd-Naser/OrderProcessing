using System.Security.Claims;

namespace OrderProcessing.WebApi.Extensions;

public static class UserExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}