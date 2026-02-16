using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.GetCategories;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class GetCategoriesTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnEmptyCollection_WhenNoCategoriesExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        GetCategoriesQuery query = new();

        // Act
        Result<IReadOnlyCollection<CategoryResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        await CleanDatabaseAsync(cancellationToken);
        await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        GetCategoriesQuery query = new();

        // Act
        Result<IReadOnlyCollection<CategoryResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count);
    }
}
