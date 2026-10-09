using OrderProcessing.Application.Auth.Commands.RegisterCustomer;
using OrderProcessing.Application.Auth.Commands.RegisterVendor;
using OrderProcessing.Application.Auth.Common;


namespace OrderProcessing.Application.Auth.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(LoginCommand request , CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegisterCustomerAsync(RegisterCustomerCommand request , CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegisterVendorAsync(RegisterVendorCommand request , CancellationToken cancellationToken = default);
  
}
