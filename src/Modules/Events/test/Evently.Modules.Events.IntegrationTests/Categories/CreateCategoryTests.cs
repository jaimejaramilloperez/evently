using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.CreateCategory;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class CreateCategoryTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_CreateCategory_WhenCommandIsValid()
    {
        // Arrange
        CreateCategoryCommand command = new("Category name");

        // Act
        Result<CategoryResponse> result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCommandIsNotValid()
    {
        // Arrange
        CreateCategoryCommand command = new("");

        // Act
        Result<CategoryResponse> result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equivalent(ErrorType.Validation, result.Error.Type);
    }
}
