using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Application.Carts.GetCart;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Carts;

internal sealed class GetCart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/carts", async (ICustomerContext customerContext, ISender sender, CancellationToken cancellationToken) =>
        {
            GetCartQuery query = new(customerContext.CustomerId);

            Result<Cart> result = await sender.Send(query, cancellationToken);

            return result.Match(() => Results.Ok(result.Value), CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCart)
        .WithTags(Tags.Carts);
    }
}
