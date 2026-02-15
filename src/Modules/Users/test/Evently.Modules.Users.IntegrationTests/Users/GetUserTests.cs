using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Application.Users.RegisterUser;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationTests.Abstractions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class GetUserTests(IntegrationTestWebAppFactory appFactory)
    : BaseIntegrationTest(appFactory)
{
    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        Guid userId = User.CreateUserId();
        GetUserQuery query = new(userId);

        // Act
        Result<UserResponse> userResult = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(userResult);
        Assert.False(userResult.IsSuccess);
        Assert.Equivalent(UserErrors.NotFound(userId), userResult.Error);
    }

    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        RegisterUserCommand command = new()
        {
            Email = Faker.Internet.Email(),
            Password = Faker.Internet.Password(),
            FirstName = Faker.Person.FirstName,
            LastName = Faker.Person.LastName,
        };

        Result<Guid> userCreatedResult = await SendAsync(command, TestContext.Current.CancellationToken);

        Guid userId = userCreatedResult.Value;
        GetUserQuery query = new(userId);

        // Act
        Result<UserResponse> userResult = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(userResult);
        Assert.True(userResult.IsSuccess);
        Assert.NotNull(userResult.Value);
        Assert.Equivalent(command.Email, userResult.Value.Email);
    }
}
