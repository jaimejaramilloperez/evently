using System.Reflection;
using Evently.Common.Domain;
using Evently.Common.Domain.DomainEvents;
using Evently.Modules.Attendance.ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using Xunit;

namespace Evently.Modules.Attendance.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent", StringComparison.Ordinal)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        List<Type> failingTypes = [];

        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

            if (!constructors.Any(x => x.IsPrivate && x.GetParameters().Length == 0))
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        IEnumerable<Type> entityTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        List<Type> failingTypes = [];

        foreach (Type entityType in entityTypes)
        {
            ConstructorInfo[] constructors = entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            if (constructors.Length != 0)
            {
                failingTypes.Add(entityType);
            }
        }

        failingTypes.Should().BeEmpty();
    }
}
