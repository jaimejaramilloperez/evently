using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Evently.Common.Domain.DomainEvents;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;
using Evently.Modules.Events.Domain.Categories.DomainEvents;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using AttendanceEventCreatedDomainEvent = Evently.Modules.Attendance.Domain.Events.DomainEvents.EventCreatedDomainEvent;
using AttendanceTicketCreatedDomainEvent = Evently.Modules.Attendance.Domain.Tickets.DomainEvents.TicketCreatedDomainEvent;
using EventsEventCreatedDomainEvent = Evently.Modules.Events.Domain.Events.DomainEvents.EventCreatedDomainEvent;
using TicketArchivedDomainEvent = Evently.Modules.Ticketing.Domain.Tickets.DomainEvents.TicketArchivedDomainEvent;
using TicketCreatedDomainEvent = Evently.Modules.Ticketing.Domain.Tickets.DomainEvents.TicketCreatedDomainEvent;

namespace Evently.Api.Serialization;

public static class DomainEventPolymorphicResolver
{
    public static void Resolver(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type == typeof(IDomainEvent) || typeInfo.Type == typeof(DomainEvent))
        {
            typeInfo.PolymorphismOptions = new()
            {
                TypeDiscriminatorPropertyName = "eventName",
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    // Users Module
                    new JsonDerivedType(typeof(UserRegisteredDomainEvent), "user_registered"),
                    new JsonDerivedType(typeof(UserProfileUpdatedDomainEvent), "user_updated"),

                    // Events Module
                    new JsonDerivedType(typeof(EventsEventCreatedDomainEvent), "event_created"),
                    new JsonDerivedType(typeof(EventCanceledDomainEvent), "event_canceled"),
                    new JsonDerivedType(typeof(EventRescheduledDomainEvent), "event_rescheduled"),
                    new JsonDerivedType(typeof(EventPublishedDomainEvent), "event_published"),
                    new JsonDerivedType(typeof(CategoryArchivedDomainEvent), "category_archived"),

                    // Ticketing Module
                    new JsonDerivedType(typeof(TicketCreatedDomainEvent), "ticket_created"),
                    new JsonDerivedType(typeof(TicketArchivedDomainEvent), "ticket_archived"),
                    new JsonDerivedType(typeof(OrderCreatedDomainEvent), "order_created"),
                    new JsonDerivedType(typeof(OrderTicketsIssuedDomainEvent), "order_tickets_issued"),
                    new JsonDerivedType(typeof(PaymentCreatedDomainEvent), "payment_created"),
                    new JsonDerivedType(typeof(PaymentRefundedDomainEvent), "payment_refunded"),
                    new JsonDerivedType(typeof(PaymentPartiallyRefundedDomainEvent), "payment_partially_refunded"),

                    // Attendance Module
                    new JsonDerivedType(typeof(AttendanceTicketCreatedDomainEvent), "attendance_ticket_created"),
                    new JsonDerivedType(typeof(TicketUsedDomainEvent), "ticket_used"),
                    new JsonDerivedType(typeof(AttendeeCheckedInDomainEvent), "attendee_checked_in"),
                    new JsonDerivedType(typeof(InvalidCheckInAttemptedDomainEvent), "invalid_check_in_attempted"),
                    new JsonDerivedType(typeof(DuplicateCheckInAttemptedDomainEvent), "duplicate_check_in_attempted"),
                    new JsonDerivedType(typeof(AttendanceEventCreatedDomainEvent), "attendance_event_created"),
                }
            };
        }
    }
}
