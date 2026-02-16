using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.Events;

public class CreateEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<Guid, string, string, string, DateTime, DateTime?> InvalidData = new()
    {
        // Invalid Guid (Empty)
        { Guid.Empty, "Rock", "Classic Rock", "123 Main St", default, default },

        // Invalid Name (Empty string)
        { Guid.Parse("d7b3f7f1-7c5a-4b6a-9f2d-1a2b3c4d5e6f"), string.Empty, "Jazz", "456 Oak Ave", default, default },

        // Invalid Genre (Empty string)
        { Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"), "Electronic", string.Empty, "789 Pine Rd", default, default },

        // Invalid Address (Empty string)
        { Guid.Parse("f9e8d7c6-b5a4-4321-8765-43210fedcba9"), "Pop", "Synth-pop", string.Empty, default, default },

        // All fields valid but testing specific boundary/default dates
        { Guid.Parse("12345678-1234-1234-1234-1234567890ab"), "Classical", "Baroque", "321 Elm St", DateTime.MinValue, null },
    };

    [Theory]
    [MemberData(nameof(InvalidData))]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid(
        Guid eventId,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        // Arrange
        CreateEventCommand command = new()
        {
            EventId = eventId,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        CreateEventCommand command = new()
        {
            EventId = Guid.CreateVersion7(),
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = DateTime.UtcNow.AddMinutes(10),
            EndsAtUtc = null,
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
