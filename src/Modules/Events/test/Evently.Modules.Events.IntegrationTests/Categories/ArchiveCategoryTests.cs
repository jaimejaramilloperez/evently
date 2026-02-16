using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.ArchiveCategory;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class ArchiveCategoryTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        ArchiveCategoryCommand command = new(Guid.NewGuid());

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CategoryErrors.NotFound(command.CategoryId), result.Error);
    }

    [Fact]
    public async Task Should_ArchiveCategory_WhenCategoryExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        ArchiveCategoryCommand command = new(categoryId);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryAlreadyArchived()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        ArchiveCategoryCommand command = new(categoryId);

        await SendAsync(command, cancellationToken);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(CategoryErrors.AlreadyArchived, result.Error);
    }
}
