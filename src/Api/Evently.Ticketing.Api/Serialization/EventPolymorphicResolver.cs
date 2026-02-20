using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Evently.Common.Application.EventBus;
using Evently.Common.Domain.DomainEvents;

namespace Evently.Ticketing.Api.Serialization;

public static class EventPolymorphicResolver
{
    public static void Resolver(JsonTypeInfo typeInfo)
    {
        JsonNamingPolicy snakeCasePolicy = JsonNamingPolicy.SnakeCaseLower;

        Type targetType = typeInfo.Type;

        if (targetType == typeof(IDomainEvent) || targetType == typeof(IIntegrationEvent))
        {
            JsonPolymorphismOptions options = new()
            {
                TypeDiscriminatorPropertyName = "eventName",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };

            Type[] derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(type => targetType.IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
                .ToArray();

            foreach (Type type in derivedTypes)
            {
                string[] namespaceParts = type.Namespace?.Split('.') ?? [];
                string moduleName = namespaceParts.Length > 2 ? namespaceParts[2].ToLowerInvariant() : "common";

                string eventName = snakeCasePolicy.ConvertName(type.Name);
                string uniqueDiscriminator = $"{moduleName}.{eventName}";

                if (!options.DerivedTypes.Any(x => x.TypeDiscriminator?.ToString() == uniqueDiscriminator))
                {
                    options.DerivedTypes.Add(new JsonDerivedType(type, uniqueDiscriminator));
                }
            }

            typeInfo.PolymorphismOptions = options;
        }
    }
}
