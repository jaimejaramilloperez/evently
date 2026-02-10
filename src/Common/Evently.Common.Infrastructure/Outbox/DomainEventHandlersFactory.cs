using System.Collections.Concurrent;
using System.Reflection;
using Evently.Common.Application.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Outbox;

public static class DomainEventHandlersFactory
{
    private static readonly ConcurrentDictionary<string, Type[]> HandlersDictionary = new();

    public static IEnumerable<IDomainEventHandler> GetHandlers(
        Type type,
        Assembly assembly,
        IServiceProvider serviceProvider)
    {
        Type[] domainEventHandlerTypes = HandlersDictionary.GetOrAdd(
            $"{assembly.GetName().Name}{type.Name}",
            _ =>
            {
                Type[] domainEventHandlerTypes = assembly.GetTypes()
                    .Where(x => x.IsAssignableTo(typeof(IDomainEventHandler<>).MakeGenericType(type)))
                    .ToArray();

                return domainEventHandlerTypes;
            });

        List<IDomainEventHandler> handlers = [];

        foreach (Type domainEventHandlerType in domainEventHandlerTypes)
        {
            object domainEventHandler = serviceProvider.GetRequiredService(domainEventHandlerType);

            if (domainEventHandler is IDomainEventHandler domainEventHandlerCast)
            {
                handlers.Add(domainEventHandlerCast);
            }
        }

        return handlers;
    }
}
