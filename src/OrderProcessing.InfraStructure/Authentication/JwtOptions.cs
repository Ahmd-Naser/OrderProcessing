using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int ExpirationInMinutes { get; init; } = 60;
}