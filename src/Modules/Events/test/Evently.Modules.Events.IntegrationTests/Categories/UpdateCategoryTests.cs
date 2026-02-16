using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class UpdateCategoryTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    public static readonly TheoryData<UpdateCategoryCommand> InvalidCommands = new()
    {
        new UpdateCategoryCommand()
        {
            CategoryId = Guid.Empty,
            Name = "movies",
        },
        new UpdateCategoryCommand()
        {
            CategoryId = Guid.CreateVersion7(),
            Name = string.Empty,
        },
    };

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Should_ReturnFailure_WhenCommandIsNotValid(UpdateCategoryCommand command)
    {
        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equivalent(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        UpdateCategoryCommand command = new()
        {
            CategoryId = Guid.CreateVersion7(),
            Name = Faker.Random.AlphaNumeric(10),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CategoryErrors.NotFound(command.CategoryId), result.Error);
    }

    [Fact]
    public async Task Should_UpdateCategory_WhenCategoryExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        UpdateCategoryCommand command = new()
        {
            CategoryId = categoryId,
            Name = Faker.Random.AlphaNumeric(10),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
