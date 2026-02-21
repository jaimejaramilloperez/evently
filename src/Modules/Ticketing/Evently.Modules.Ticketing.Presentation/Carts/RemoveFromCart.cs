using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class RemoveFromCart : IEndpoint
{
    internal sealed class Request
    {
        public Guid TicketTypeId { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/carts/remove", async (Request request, ICustomerContext customerContext, ISender sender, CancellationToken cancellationToken) =>
        {
            RemoveItemFromCartCommand command = new()
            {
                CustomerId = customerContext.CustomerId,
                TicketTypeId = request.TicketTypeId,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.RemoveFromCart)
        .WithTags(Tags.Carts);
    }
}
