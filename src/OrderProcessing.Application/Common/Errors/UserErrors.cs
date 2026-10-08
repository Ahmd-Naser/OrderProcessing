using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing.Application.Common.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid username Or password", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error DisabledUser =
       new("User.DisabledUser", "Disabled User , please contact your administrator", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error LockedUser =
       new("User.LockedUser", "Locked User , please contact your administrator", (int)HttpStatusCodes.Unauthorized);


    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Token", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Operation Failed", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmail", "Another user with the same email is already exist", (int)HttpStatusCodes.Conflict);

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email is not confirmed", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error InvalidCode =
        new("User.InvalidCode", "the code is invalid", (int)HttpStatusCodes.Unauthorized);

    public static readonly Error DuplicatedConfirmation =
        new("User.DuplicatedConfirmation", "Email already confirmed", (int)HttpStatusCodes.BadRequest);

    public static readonly Error NotFound =
        new("User.NotFound", "User is not found", (int)HttpStatusCodes.NotFound);

    public static readonly Error InvalidRoles =
        new("User.InvalidRoles", "Invalid Roles", (int)HttpStatusCodes.BadRequest);
}