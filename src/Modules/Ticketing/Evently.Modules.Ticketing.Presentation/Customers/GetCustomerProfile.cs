using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Abstractions.Authentication;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Ticketing.Presentation.Customers;

internal sealed class GetCustomerProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/customers/profile", async (ICustomerContext customerContext, ISender sender, CancellationToken cancellationToken) =>
        {
            GetCustomerQuery query = new(customerContext.CustomerId);

            Result<CustomerResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.GetCustomer)
        .WithTags(Tags.Customers);
    }
}
