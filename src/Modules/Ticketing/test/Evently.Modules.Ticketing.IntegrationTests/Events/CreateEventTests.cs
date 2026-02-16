using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Events;

public class CreateEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCreated()
    {
        // Arrange
        Guid eventId = Guid.CreateVersion7();
        Guid ticketTypeId = Guid.CreateVersion7();
        decimal quantity = Faker.Random.Decimal();

        CreateEventCommand.TicketTypeRequest ticketType = new()
        {
            TicketTypeId = ticketTypeId,
            EventId = eventId,
            Name = Faker.Random.AlphaNumeric(10),
            Price = Faker.Random.Decimal(),
            Currency = Faker.Random.AlphaNumeric(3),
            Quantity = quantity,
        };

        CreateEventCommand command = new()
        {
            EventId = eventId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Address.FullAddress(),
            StartsAtUtc = DateTime.UtcNow,
            EndsAtUtc = null,
            TicketTypes = [ticketType],
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
