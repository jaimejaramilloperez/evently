using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class AddToCart : IEndpoint
{
    internal sealed record Request
    {
        public Guid TicketTypeId { get; init; }
        public decimal Quantity { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/carts/add", async (Request request, ICustomerContext customerContext, ISender sender, CancellationToken cancellationToken) =>
        {
            AddItemToCartCommand command = new()
            {
                CustomerId = customerContext.CustomerId,
                TicketTypeId = request.TicketTypeId,
                Quantity = request.Quantity,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(() => Results.NoContent(), CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.AddToCart)
        .WithTags(Tags.Carts);
    }
}
