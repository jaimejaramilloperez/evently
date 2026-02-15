using System.Net;
using Evently.Modules.Users.IntegrationTests.Abstractions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class GetUserProfileTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnUnauthorized_WhenAccessTokenNotProvided()
    {
        // Arrange
        HttpClient httpClient = CreateClient();

        // Act
        HttpResponseMessage response = await httpClient.GetAsync(
            new Uri("/api/users/profile", UriKind.Relative),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
