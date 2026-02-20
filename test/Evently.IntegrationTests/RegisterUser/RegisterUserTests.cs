using System.Net.Http.Json;
using Evently.Common.Domain.Results;
using Evently.IntegrationTests.Abstractions;
using Evently.Modules.Attendance.Application.Attendees.GetAttendee;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Evently.IntegrationTests.RegisterUser;

public class RegisterUserTests(
    ApiIntegrationTestWebAppFactory apiFactory,
    TicketingApiIntegrationTestWebAppFactory ticketingApiFactory)
    : BaseIntegrationTest(apiFactory, ticketingApiFactory)
{
    [Fact]
    public async Task RegisterUser_Should_PropagateToAttendanceModule()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        string email = Faker.Internet.Email();
        string password = Faker.Random.AlphaNumeric(8);

        Result<Guid> userResult = await RegisterUserAsync(email, password, cancellationToken);
        Assert.True(userResult.IsSuccess);

        string accessToken = await GetAccessTokenAsync(email, password, cancellationToken);

        // Act
        HttpResponseMessage response = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                using HttpRequestMessage message = new(HttpMethod.Get, "/api/attendees/profile");
                message.Headers.Authorization = new(JwtBearerDefaults.AuthenticationScheme, accessToken);

                HttpResponseMessage response = await ApiClient.SendAsync(message, cancellationToken);

                return response;
            },
            cancellationToken);

        AttendeeResponse? attendeeResponse = await response.Content.ReadFromJsonAsync<AttendeeResponse>(cancellationToken);

        // Assert
        Assert.NotNull(attendeeResponse);
        Assert.Equal(userResult.Value, attendeeResponse.Id);
    }

    [Fact]
    public async Task RegisterUser_Should_PropagateToTicketingModule()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        string email = Faker.Internet.Email();
        string password = Faker.Random.AlphaNumeric(8);

        Result<Guid> userResult = await RegisterUserAsync(email, password, cancellationToken);
        Assert.True(userResult.IsSuccess);

        string accessToken = await GetAccessTokenAsync(email, password, cancellationToken);

        // Act
        HttpResponseMessage response = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                using HttpRequestMessage message = new(HttpMethod.Get, "/api/customers/profile");
                message.Headers.Authorization = new(JwtBearerDefaults.AuthenticationScheme, accessToken);

                HttpResponseMessage response = await TicketingApiClient.SendAsync(message, cancellationToken);

                return response;
            },
            cancellationToken);

        CustomerResponse? customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>(cancellationToken);

        // Assert
        Assert.NotNull(customerResponse);
        Assert.Equal(userResult.Value, customerResponse.Id);
    }
}
