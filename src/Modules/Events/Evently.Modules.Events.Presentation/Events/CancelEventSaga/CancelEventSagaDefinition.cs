using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

public sealed class CancelEventSagaDefinition : MassTransitStateMachine<CancelEventState>
{
    public State CancellationStarted { get; private set; } = null!;
    public State PaymentsRefunded { get; private set; } = null!;
    public State TicketsArchived { get; private set; } = null!;

    public Event<EventCanceledIntegrationEvent> EventCanceled { get; private set; } = null!;
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; } = null!;
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; } = null!;
    public Event EventCancellationCompleted { get; private set; } = null!;

    public CancelEventSagaDefinition()
    {
        Event(() => EventCanceled, c => c.CorrelateById(m => m.Message.EventId));
        Event(() => EventPaymentsRefunded, c => c.CorrelateById(m => m.Message.EventId));
        Event(() => EventTicketsArchived, c => c.CorrelateById(m => m.Message.EventId));

        InstanceState(s => s.CurrentState);

        Initially(
            When(EventCanceled)
                .Publish(context =>
                    new EventCancellationStartedIntegrationEvent(
                        context.Message.Id,
                        context.Message.OccurredAtUtc,
                        context.Message.EventId))
                .TransitionTo(CancellationStarted));

        During(CancellationStarted,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded),
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(PaymentsRefunded,
            When(EventTicketsArchived)
                .TransitionTo(TicketsArchived));

        During(TicketsArchived,
            When(EventPaymentsRefunded)
                .TransitionTo(PaymentsRefunded));

        CompositeEvent(
            () => EventCancellationCompleted,
            state => state.CancellationCompletedStatus,
            EventPaymentsRefunded, EventTicketsArchived);

        DuringAny(
            When(EventCancellationCompleted)
                .Publish(context =>
                    new EventCancellationCompletedIntegrationEvent(
                        Guid.CreateVersion7(),
                        DateTime.UtcNow,
                        context.Saga.CorrelationId))
                .Finalize());
    }
}
