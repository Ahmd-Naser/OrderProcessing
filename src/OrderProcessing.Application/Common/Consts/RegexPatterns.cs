using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Consts;

public static class RegexPatterns
{
    public const string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
}