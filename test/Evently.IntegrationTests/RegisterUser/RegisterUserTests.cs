using Evently.Common.Domain.Results;
using Evently.IntegrationTests.Abstractions;
using Evently.Modules.Attendance.Application.Attendees.GetAttendee;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;

namespace Evently.IntegrationTests.RegisterUser;

public class RegisterUserTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task RegisterUser_Should_PropagateToTicketingModule()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Result<Guid> userResult = await RegisterUserAsync(cancellationToken);
        Assert.True(userResult.IsSuccess);

        // Act
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                GetCustomerQuery query = new(userResult.Value);

                Result<CustomerResponse> customerResult = await SendAsync(query, cancellationToken);

                return customerResult;
            });

        // Assert
        Assert.True(customerResult.IsSuccess);
        Assert.NotNull(customerResult.Value);
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToAttendanceModule()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Result<Guid> userResult = await RegisterUserAsync(cancellationToken);
        Assert.True(userResult.IsSuccess);

        // Act
        Result<AttendeeResponse> attendeeResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                GetAttendeeQuery query = new(userResult.Value);

                Result<AttendeeResponse> customerResult = await SendAsync(query, cancellationToken);

                return customerResult;
            });

        // Assert
        Assert.True(attendeeResult.IsSuccess);
        Assert.NotNull(attendeeResult.Value);
    }
}
