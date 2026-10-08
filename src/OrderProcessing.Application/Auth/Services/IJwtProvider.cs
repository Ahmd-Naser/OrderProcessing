using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Services;

public interface IJwtProvider
{
    (string Token, int ExpiresIn) GenerateToken(
        string userId,
        string email,
        IEnumerable<string> roles);
}
