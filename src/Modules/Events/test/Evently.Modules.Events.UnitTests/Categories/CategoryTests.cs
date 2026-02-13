using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Categories.DomainEvents;
using Evently.Modules.Events.UnitTests.Abstractions;

namespace Evently.Modules.Events.UnitTests.Categories;

public class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
    {
        // Act
        Result<Category> sut = Category.Create(Faker.Music.Genre());

        // Assert
        Assert.NotNull(sut);
        Assert.True(sut.IsSuccess);
        Assert.NotNull(sut.Value);

        CategoryCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(sut.Value);
        Assert.Equivalent(sut.Value.Id, domainEvent.CategoryId);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
    {
        // Arrange
        Result<Category> result = Category.Create(Faker.Music.Genre());

        Category sut = result.Value;

        // Act
        sut.Archive();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        CategoryArchivedDomainEvent domainEvent = AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(sut);
        Assert.Equivalent(sut.Id, domainEvent.CategoryId);
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
    {
        // Arrange
        Result<Category> result = Category.Create(Faker.Music.Genre());
        Category sut = result.Value;

        string newName = Faker.Music.Genre();

        // Act
        sut.ChangeName(newName);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        CategoryNameChangedDomainEvent domainEvent = AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(sut);
        Assert.Equivalent(sut.Id, domainEvent.CategoryId);
        Assert.Equivalent(newName, domainEvent.Name);
    }
}
