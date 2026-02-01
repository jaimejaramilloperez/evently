using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrder;

internal sealed class GetOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrderQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                o.id AS {nameof(OrderResponse.Id)},
                o.customer_id AS {nameof(OrderResponse.CustomerId)},
                o.status AS {nameof(OrderResponse.Status)},
                o.total_price AS {nameof(OrderResponse.TotalPrice)},
                o.created_at_utc AS {nameof(OrderResponse.CreatedAtUtc)},
                oi.id AS {nameof(OrderItemResponse.OrderItemId)},
                oi.order_id AS {nameof(OrderItemResponse.OrderId)},
                oi.ticket_type_id AS {nameof(OrderItemResponse.TicketTypeId)},
                oi.quantity AS {nameof(OrderItemResponse.Quantity)},
                oi.unit_price AS {nameof(OrderItemResponse.UnitPrice)},
                oi.price AS {nameof(OrderItemResponse.Price)},
                oi.currency AS {nameof(OrderItemResponse.Currency)}
            FROM
                ticketing.orders AS o
            JOIN
                ticketing.order_items AS oi ON oi.order_id = o.id
            WHERE
                o.id = @OrderId
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { request.OrderId },
            cancellationToken: cancellationToken);

        Dictionary<Guid, OrderResponse> ordersDictionary = [];
        await connection.QueryAsync<OrderResponse, OrderItemResponse, OrderResponse>(
            command: command,
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out OrderResponse? existingEvent))
                {
                    order = existingEvent;
                }
                else
                {
                    ordersDictionary.Add(order.Id, order);
                }

                order.OrderItems.Add(orderItem);

                return order;
            },
            splitOn: nameof(OrderItemResponse.OrderItemId));

        if (!ordersDictionary.TryGetValue(request.OrderId, out OrderResponse? orderResponse))
        {
            return Result.Failure<OrderResponse>(OrderErrors.NotFound(request.OrderId));
        }

        return orderResponse;
    }
}
