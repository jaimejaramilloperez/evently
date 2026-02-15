using System.Net;
using System.Net.Http.Json;
using Evently.Modules.Users.IntegrationTests.Abstractions;
using Evently.Modules.Users.Presentation.Users;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class RegisterUserTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<string, string, string, string> InvalidRequests = new()
    {
        { "", "password", "firstName", "lastName" },
        { "email@mail.com", "", "firstName", "lastName" },
        { "email@mail.com", "password", "", "lastName" },
        { "email@mail.com", "password", "firstName", "" },
    };

    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public async Task Should_ReturnBadRequest_WhenRequestIsNotValid(
        string email,
        string password,
        string firstName,
        string lastName)
    {
        // Arrange
        using HttpClient client = CreateClient();

        RegisterUser.Request request = new()
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
        };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/users/register",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        HttpClient client = CreateClient();

        RegisterUser.Request request = new()
        {
            Email = "create@test.com",
            Password = Faker.Internet.Password(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/users/register",
            request,
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_ReturnAccessToken_WhenUserIsRegistered()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        HttpClient client = CreateClient();

        RegisterUser.Request request = new()
        {
            Email = "token@test.com",
            Password = Faker.Internet.Password(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        await client.PostAsJsonAsync("/api/users/register", request, cancellationToken);

        // Act
        string accessToken = await GetAccessTokenAsync(request.Email, request.Password, cancellationToken);

        // Assert
        Assert.NotEmpty(accessToken);
    }
}
