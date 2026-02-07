using System.Security.Claims;
using Evently.Common.Domain.Results;
using Evently.Common.Infrastructure.Authentication;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Users.Presentation.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    internal sealed record Request
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/profile", async (Request request, ClaimsPrincipal user, ISender sender, CancellationToken cancellationToken) =>
        {
            Guid userId = user.GetUserId();

            UpdateUserCommand command = new()
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyUser)
        .WithTags(Tags.Users);
    }
}
