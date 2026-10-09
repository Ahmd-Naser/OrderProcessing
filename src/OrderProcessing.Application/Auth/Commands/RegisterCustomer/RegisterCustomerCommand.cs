using OrderProcessing.Application.Auth.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.RegisterCustomer;

public record RegisterCustomerCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<Result<AuthResponse>>;