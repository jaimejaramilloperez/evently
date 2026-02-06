using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;
using Evently.Modules.Ticketing.Application.Tickets.GetTicketByCode;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Tickets;

internal sealed class GetTicketByCode : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets/code/{code}", async (string code, ISender sender, CancellationToken cancellationToken) =>
        {
            GetTicketByCodeQuery query = new(code);

            Result<TicketResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Tickets);
    }
}
