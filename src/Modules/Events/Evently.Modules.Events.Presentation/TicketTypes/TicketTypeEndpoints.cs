using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public static class TicketTypeEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder ticketTypesGroup = app.MapGroup("/ticket-types")
            .WithTags(Tags.TicketTypes);

        ChangeTicketTypePrice.MapEndpoint(ticketTypesGroup);
        CreateTicketType.MapEndpoint(ticketTypesGroup);
        GetTicketType.MapEndpoint(ticketTypesGroup);
        GetTicketTypes.MapEndpoint(ticketTypesGroup);
    }
}
