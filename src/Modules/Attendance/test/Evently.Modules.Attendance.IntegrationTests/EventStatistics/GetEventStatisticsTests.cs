using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.EventStatistics.GetEventStatistics;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.EventStatistics;

public class GetEventStatisticsTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenEventStatisticsDoesNotExist()
    {
        // Arrange
        GetEventStatisticsQuery query = new(Guid.CreateVersion7());

        // Act
        Result<EventStatisticsResponse> result = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(query.EventId), result.Error);
    }
}
