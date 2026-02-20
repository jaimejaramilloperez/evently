using Evently.Common.Application.Authorization;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Users.Presentation.Users;

public sealed class GetUserPermissionRequestConsumer(IPermissionService permissionService)
    : IConsumer<GetUserPermissionRequest>
{
    public async Task Consume(ConsumeContext<GetUserPermissionRequest> context)
    {
        string identityId = context.Message.IdentityId;

        Result<PermissionsResponse> result = await permissionService.GetUserPermissionsAsync(identityId);

        if (!result.IsSuccess)
        {
            await context.RespondAsync(result.Error);
            return;
        }

        await context.RespondAsync(result.Value);
    }
}
