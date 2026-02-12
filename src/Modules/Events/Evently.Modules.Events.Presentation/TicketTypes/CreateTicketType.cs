using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class CreateTicketType : IEndpoint
{
    internal sealed class Request
    {
        public required Guid EventId { get; init; }
        public required string Name { get; init; }
        public required decimal Price { get; init; }
        public required string Currency { get; init; }
        public required decimal Quantity { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/ticket-types", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            CreateTicketTypeCommand command = new()
            {
                EventId = request.EventId,
                Name = request.Name,
                Price = request.Price,
                Currency = request.Currency,
                Quantity = request.Quantity,
            };

            Result<TicketTypeResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(() =>
                Results.Created(new Uri($"/api/ticket-types/{result.Value.Id}", UriKind.Relative), result.Value),
                CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyTicketTypes)
        .WithTags(Tags.TicketTypes);
    }
}
