using Evently.Modules.Events.Presentation.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation;

public static class EventEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder eventsGroup = app.MapGroup("/events")
            .WithTags(Tags.Events);

        GetEvent.MapEndpoint(eventsGroup);
        CreateEvent.MapEndpoint(eventsGroup);
    }
}
