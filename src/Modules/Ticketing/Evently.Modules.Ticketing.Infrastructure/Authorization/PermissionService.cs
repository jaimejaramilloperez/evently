using Evently.Common.Application.Authorization;
using Evently.Common.Application.Caching;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Ticketing.Infrastructure.Authorization;

internal sealed class PermissionService(
    IRequestClient<GetUserPermissionRequest> requestClient,
    ICacheService cacheService)
    : IPermissionService
{
    private static readonly Error NotFound = Error.NotFound(nameof(PermissionService), "The user wa not found");
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        string cacheKey = CreateCacheKey(identityId);

        PermissionsResponse? permissionsResponse = await cacheService.GetAsync<PermissionsResponse>(cacheKey);

        if (permissionsResponse is not null)
        {
            return permissionsResponse;
        }

        GetUserPermissionRequest request = new(identityId);

        Response<PermissionsResponse, Error> response = await requestClient.GetResponse<PermissionsResponse, Error>(request);

        if (response.Is(out Response<Error>? errorResponse))
        {
            return Result.Failure<PermissionsResponse>(errorResponse.Message);
        }

        if (response.Is(out Response<PermissionsResponse>? permissionResponse))
        {
            await cacheService.SetAsync(cacheKey, permissionResponse.Message, CacheExpiration);
            return permissionResponse.Message;
        }

        return Result.Failure<PermissionsResponse>(NotFound);
    }

    private static string CreateCacheKey(string identityId) => $"user-permissions:{identityId}";
}
