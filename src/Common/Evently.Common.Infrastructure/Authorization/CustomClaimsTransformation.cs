using System.Security.Claims;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Common.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory)
    : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(x => x.Type == CustomClaims.Sub))
        {
            return principal;
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        string identityId = principal.GetIdentityId();

        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(IPermissionService.GetUserPermissionsAsync), result.Error);
        }

        ClaimsIdentity claimsIdentity = new();

        claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));
        claimsIdentity.AddClaims(result.Value.Permissions.Select(permission => new Claim(CustomClaims.Permission, permission)));

        principal.AddIdentity(claimsIdentity);

        return principal;
    }
}
