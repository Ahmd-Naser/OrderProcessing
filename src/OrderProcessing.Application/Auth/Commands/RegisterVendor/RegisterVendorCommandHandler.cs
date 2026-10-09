using OrderProcessing.Application.Auth.Commands.RegisterCustomer;
using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.RegisterVendor;

public class RegisterVendorCommandHandler(IAuthService authService) : IRequestHandler<RegisterVendorCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService = authService;

    public Task<Result<AuthResponse>> Handle(RegisterVendorCommand request, CancellationToken cancellationToken)
    {
        var result = _authService.RegisterVendorAsync(request, cancellationToken);

        return result;
    }
}
