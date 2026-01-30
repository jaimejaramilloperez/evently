using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.PublicApi;
using MediatR;
using UserResponse = Evently.Modules.Users.PublicApi.UserResponse;

namespace Evently.Modules.Users.Infrastructure.PublicApi;

internal sealed class UsersApi(ISender sender) : IUsersApi
{
    public async Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        GetUserQuery query = new(userId);

        Result<Application.Users.GetUser.UserResponse> result = await sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return null;
        }

        return new()
        {
            Id = result.Value.Id,
            Email = result.Value.Email,
            FirstName = result.Value.FirstName,
            LastName = result.Value.LastName,
        };
    }
}
