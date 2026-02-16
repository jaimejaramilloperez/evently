using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Categories;

public class GetCategoryTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        GetCategoryQuery query = new(Guid.NewGuid());

        // Act
        Result result = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CategoryErrors.NotFound(query.CategoryId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnCategory_WhenCategoryExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        GetCategoryQuery query = new(categoryId);

        // Act
        Result<CategoryResponse> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
