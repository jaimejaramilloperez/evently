using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.Attendees;

public class UpdateAttendeeTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange
        UpdateAttendeeCommand command = new()
        {
            AttendeeId = Guid.NewGuid(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(AttendeeErrors.NotFound(command.AttendeeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid attendeeId = await CreateAttendeeAsync(Guid.CreateVersion7(), cancellationToken);

        UpdateAttendeeCommand command = new()
        {
            AttendeeId = attendeeId,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
