using Microsoft.AspNetCore.Identity;
using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using OrderProcessing.Application.Common.Models;
using OrderProcessing.Persistence.Identity.Models;


namespace OrderProcessing.Persistence.Identity.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider

) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public Task<Result<AuthResponse>> GetTokenAsync(LoginCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AuthResponse>> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
