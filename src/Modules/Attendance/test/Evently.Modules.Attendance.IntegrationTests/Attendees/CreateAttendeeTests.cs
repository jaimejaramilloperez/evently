using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.Attendees;

public class CreateAttendeeTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsInvalid()
    {
        // Arrange
        CreateAttendeeCommand command = new()
        {
            AttendeeId = Guid.CreateVersion7(),
            Email = string.Empty,
            FirstName = string.Empty,
            LastName = string.Empty,
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
        CreateAttendeeCommand command = new()
        {
            AttendeeId = Guid.CreateVersion7(),
            Email = Faker.Internet.Email(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
