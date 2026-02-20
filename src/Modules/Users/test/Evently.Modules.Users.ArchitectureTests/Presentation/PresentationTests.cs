using Evently.Modules.Users.ArchitectureTests.Abstractions;
using MassTransit;
using NetArchTest.Rules;
using Xunit;

namespace Evently.Modules.Users.ArchitectureTests.Presentation;

public class PresentationTests : BaseTest
{
    [Fact]
    public void IntegrationEventConsumer_Should_BeSealed()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void IntegrationEventConsumer_ShouldHave_NameEndingWith_IntegrationEventConsumer()
    {
        Types.InAssembly(PresentationAssembly)
            .That()
            .ImplementInterface(typeof(IConsumer<>))
            .Should()
            .HaveNameEndingWith("IntegrationEventConsumer", StringComparison.Ordinal)
            .Or()
            .HaveNameEndingWith("Consumer", StringComparison.Ordinal)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
