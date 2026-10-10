using OrderProcessing.Application.Auth.Common;
using OrderProcessing.Application.Auth.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.Login;

public class LoginCommandHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IAuthService _authService = authService;

    public Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        => _authService.GetTokenAsync(request, cancellationToken);
}
