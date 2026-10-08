using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Common;

public record AuthResponse(
    string Id,
    string Email,
    string Token,
    int ExpiresIn
);