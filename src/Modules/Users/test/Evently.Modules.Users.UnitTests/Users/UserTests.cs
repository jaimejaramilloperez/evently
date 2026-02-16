using Evently.Modules.Users.Domain.Roles;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.UnitTests.Abstractions;

namespace Evently.Modules.Users.UnitTests.Users;

public class UserTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnUser()
    {
        // Act
        User user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid());

        // Assert
        Assert.NotNull(user);
    }

    [Fact]
    public void Create_ShouldReturnUser_WithMemberRole()
    {
        // Act
        User user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid());

        // Assert
        Assert.Equal(Role.Member, user.Roles.First());
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenUserCreated()
    {
        // Act
        User user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid());

        // Assert
        UserRegisteredDomainEvent domainEvent = AssertDomainEventWasPublished<UserRegisteredDomainEvent>(user);
        Assert.Equal(user.Id, domainEvent.UserId);
    }

    [Fact]
    public void Update_ShouldRaiseDomainEvent_WhenUserUpdated()
    {
        // Arrange
        User user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid());

        // Act
        user.Update(user.LastName, user.FirstName);

        // Assert
        UserProfileUpdatedDomainEvent domainEvent = AssertDomainEventWasPublished<UserProfileUpdatedDomainEvent>(user);

        Assert.Equal(user.Id, domainEvent.UserId);
        Assert.Equal(user.FirstName, domainEvent.FirstName);
        Assert.Equal(user.LastName, domainEvent.LastName);
    }

    [Fact]
    public void Update_ShouldNotRaiseDomainEvent_WhenUserNotUpdated()
    {
        // Arrange
        User user = User.Create(
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName(),
            Guid.NewGuid());

        user.ClearDomainEvents();

        // Act
        user.Update(user.FirstName, user.LastName);

        // Assert
        Assert.Empty(user.GetDomainEvents());
    }
}
