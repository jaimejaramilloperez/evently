using Bogus;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Attendance.Application.Tickets.CreateTicket;
using Evently.Modules.Attendance.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Attendance.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory appFactory) : IDisposable
{
    protected static readonly Faker Faker = new();

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        return await sender.Send(request, cancellationToken);
    }

    public async Task CleanDatabaseAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        AttendanceDbContext dbContext = scope.ServiceProvider.GetRequiredService<AttendanceDbContext>();

        await dbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM attendance.inbox_message_consumers;
            DELETE FROM attendance.inbox_messages;
            DELETE FROM attendance.outbox_message_consumers;
            DELETE FROM attendance.outbox_messages;
            DELETE FROM attendance.attendees;
            DELETE FROM attendance.events;
            DELETE FROM attendance.tickets;
            DELETE FROM attendance.event_statistics;
            """, cancellationToken);
    }

    public async Task<Guid> CreateAttendeeAsync(Guid attendeeId, CancellationToken cancellationToken)
    {
        Result result = await SendAsync(new CreateAttendeeCommand()
        {
            AttendeeId = attendeeId,
            Email = Faker.Internet.Email(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        }, cancellationToken);

        Assert.True(result.IsSuccess);

        return attendeeId;
    }

    public async Task<Guid> CreateTicketAsync(
        Guid ticketId,
        Guid attendeeId,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        Result result = await SendAsync(new CreateTicketCommand()
        {
            TicketId = ticketId,
            AttendeeId = attendeeId,
            EventId = eventId,
            Code = Guid.CreateVersion7().ToString(),
        }, cancellationToken);

        Assert.True(result.IsSuccess);

        return ticketId;
    }

    public async Task<Guid> CreateEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        Result result = await SendAsync(new CreateEventCommand()
        {
            EventId = eventId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = DateTime.UtcNow.AddMinutes(10),
            EndsAtUtc = null,
        }, cancellationToken);

        Assert.True(result.IsSuccess);

        return eventId;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
