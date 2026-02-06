using System.Net;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "Password";

    public async Task<Result<Guid>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        UserRepresentation userRepresentation = new()
        {
            Username = user.Email,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            EmailVerified = true,
            Enabled = true,
            Credentials = [
                new()
                {
                    Type = PasswordCredentialType,
                    Value = user.Password,
                    Temporary = false,
                }
            ],
        };

        try
        {
            Guid identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);
            return identityId;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            logger.LogError(ex, "User registration failed");
            return Result.Failure<Guid>(IdentityProviderErrors.EmailIsNotUnique);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "User registration failed");
            return Result.Failure<Guid>(IdentityProviderErrors.UnexpectedError);
        }
    }
}
