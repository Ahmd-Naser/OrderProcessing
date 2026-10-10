using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Common;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<AuthResponse>>;
