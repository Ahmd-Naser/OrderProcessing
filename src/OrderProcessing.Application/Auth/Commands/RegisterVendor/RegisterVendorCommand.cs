using OrderProcessing.Application.Auth.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Commands.RegisterVendor;

public record RegisterVendorCommand(
    string StoreName,
    string Email,
    string Password

) : IRequest<Result<AuthResponse>> ;