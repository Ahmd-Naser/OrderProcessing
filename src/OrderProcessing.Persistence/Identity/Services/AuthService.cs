using Microsoft.AspNetCore.Identity;
using OrderProcessing.Application.Auth.Commands.RegisterCustomer;
using OrderProcessing.Application.Auth.Commands.RegisterVendor;
using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using OrderProcessing.Application.Common.Errors;
using OrderProcessing.Application.Common.Models;
using OrderProcessing.Domain.Consts;
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

    private async Task<Result<AuthResponse>> CreateUserInternalAsync(ApplicationUser user , string password , string role, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(user.Email!) is not null )
            return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);


        var createResult = await _userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            var firstError = createResult.Errors.First();
            return Result.Failure<AuthResponse>(new Error(firstError.Code, firstError.Description, (int)HttpStatusCodes.BadRequest ));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role );

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            var firstError = roleResult.Errors.First();
            return Result.Failure<AuthResponse>(new Error(firstError.Code, firstError.Description, (int)HttpStatusCodes.BadRequest ));
        }


        var roles = await _userManager.GetRolesAsync(user);

        var (token, expiresIn) = _jwtProvider.GenerateToken(user.Id, user.Email!, roles);

        var response = new AuthResponse(user.Id,
            user.Email!, token, expiresIn
        );

        return Result.Success(response);

    }

    public async Task<Result<AuthResponse>> RegisterCustomerAsync(
        RegisterCustomerCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        return await CreateUserInternalAsync(user, request.Password, DefaultRoles.Customer);
    }

    public async Task<Result<AuthResponse>> RegisterVendorAsync(
        RegisterVendorCommand request,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            StoreName = request.StoreName
        };

        return await CreateUserInternalAsync(user, request.Password, DefaultRoles.Vendor);
    }
}
