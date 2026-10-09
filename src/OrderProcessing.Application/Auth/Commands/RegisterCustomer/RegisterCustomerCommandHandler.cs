using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.RegisterCustomer;

public class RegisterCustomerCommandHandler(IAuthService authService) : IRequestHandler<RegisterCustomerCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService = authService;

    public Task<Result<AuthResponse>> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var result = _authService.RegisterCustomerAsync(request, cancellationToken);

        return result;
    }
}
