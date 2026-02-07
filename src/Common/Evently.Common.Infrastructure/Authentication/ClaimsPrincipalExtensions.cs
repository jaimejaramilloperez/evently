using System.Security.Claims;
using Evently.Common.Application.Exceptions;

namespace Evently.Common.Infrastructure.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(CustomClaims.Sub);

        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            throw new EventlyException("User identifier is unavailable");
        }

        return parsedUserId;
    }

    public static string GetIdentityId(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new EventlyException("User identity is unavailable");
    }

    public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
    {
        IEnumerable<Claim> permissionClaims = principal?.FindAll(CustomClaims.Permission)
            ?? throw new EventlyException("Permissions are unavailable");

        return permissionClaims.Select(x => x.Value).ToHashSet();
    }
}
