using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Users.RegisterUser;
using Evently.Modules.Users.Application.Users.UpdateUser;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.IntegrationTests.Abstractions;

namespace Evently.Modules.Users.IntegrationTests.Users;

public class UpdateUserTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<UpdateUserCommand> InvalidCommands =
    [
        new UpdateUserCommand()
        {
            UserId = Guid.Empty,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        },
        new UpdateUserCommand()
        {
            UserId = User.CreateUserId(),
            FirstName = "",
            LastName = Faker.Name.LastName(),
        },
        new UpdateUserCommand()
        {
            UserId = User.CreateUserId(),
            FirstName = Faker.Name.FirstName(),
            LastName = "",
        },
    ];

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnError_WhenCommandIsNotValid(UpdateUserCommand command)
    {
        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        Guid userId = User.CreateUserId();

        UpdateUserCommand command = new()
        {
            UserId = userId,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result updateResult = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(UserErrors.NotFound(userId), updateResult.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUserExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        RegisterUserCommand registerCommand = new()
        {
            Email = Faker.Internet.Email(),
            Password = Faker.Internet.Password(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        Result<Guid> result = await SendAsync(registerCommand, cancellationToken);
        Guid userId = result.Value;

        // Act
        UpdateUserCommand updateCommand = new()
        {
            UserId = userId,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        Result updateResult = await SendAsync(updateCommand, cancellationToken);

        // Assert
        Assert.True(updateResult.IsSuccess);
    }
}
