using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder eventsGroup = app.MapGroup("/events")
            .WithTags(Tags.Events);

        CancelEvent.MapEndpoint(eventsGroup);
        CreateEvent.MapEndpoint(eventsGroup);
        GetEvent.MapEndpoint(eventsGroup);
        GetEvents.MapEndpoint(eventsGroup);
        PublishEvent.MapEndpoint(eventsGroup);
        RescheduleEvent.MapEndpoint(eventsGroup);
        SearchEvents.MapEndpoint(eventsGroup);
    }
}
