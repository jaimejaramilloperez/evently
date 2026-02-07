using Evently.Common.Application.Authorization;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Users.GetUserPermissions;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        GetUserPermissionsQuery query = new(identityId);

        Result<PermissionsResponse> result = await sender.Send(query);

        return result;
    }
}
