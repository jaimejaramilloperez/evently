using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class GetTicketTypes : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/ticket-types", async (Guid eventId, ISender sender, CancellationToken cancellationToken) =>
        {
            GetTicketTypesQuery query = new(eventId);

            Result<IReadOnlyCollection<TicketTypeResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.TicketTypes);
    }
}
