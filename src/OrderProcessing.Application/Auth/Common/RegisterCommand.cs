using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Auth.Common;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Role
);
