using Evently.Common.Domain.Results;

namespace Evently.Modules.Users.Application.Abstractions.Identity;

public static class IdentityProviderErrors
{
    public static readonly Error EmailIsNotUnique = Error.Conflict(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique");

    public static readonly Error UnexpectedError = Error.Failure(
        "Identity.UnexpectedError",
        "An unexpected error has occurred");
}
