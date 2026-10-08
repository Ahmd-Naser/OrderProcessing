using OrderProcessing.Application.Auth.Common;


namespace OrderProcessing.Application.Auth.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(LoginCommand request , CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegisterAsync(RegisterCommand request , CancellationToken cancellationToken = default);
  
}
