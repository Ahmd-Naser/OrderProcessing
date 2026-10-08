using Microsoft.AspNetCore.Identity;
using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Models;
using OrderProcessing.Persistence.Identity.Models;


namespace OrderProcessing.Persistence.Identity.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider

) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    //private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<AuthResponse>> GetTokenAsync(LoginCommand request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var roles = await _userManager.GetRolesAsync(user);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user.Id , user.Email! , roles);

        var response = new AuthResponse(user.Id,
            user.Email!, token, expiresIn 
        );

        return Result.Success(response);
    }

    public Task<Result<AuthResponse>> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
