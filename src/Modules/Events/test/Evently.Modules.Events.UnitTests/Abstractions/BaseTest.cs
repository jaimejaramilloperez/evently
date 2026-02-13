using Bogus;
using Evently.Common.Domain;
using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Events.UnitTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Faker Faker = new();

    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        T? domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

        if (domainEvent is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} was not published");
        }

        return domainEvent;
    }
}
