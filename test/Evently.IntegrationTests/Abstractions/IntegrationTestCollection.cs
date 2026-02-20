namespace Evently.IntegrationTests.Abstractions;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public sealed class IntegrationTestCollection
    : ICollectionFixture<ApiIntegrationTestWebAppFactory>,
    ICollectionFixture<TicketingApiIntegrationTestWebAppFactory>;
