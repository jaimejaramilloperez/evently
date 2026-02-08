using System.Reflection;
using Evently.ArchitectureTests.Abstractions;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure;
using NetArchTest.Rules;
using Xunit;

namespace Evently.ArchitectureTests.Layers;

public class ModuleTest : BaseTest
{
    [Fact]
    public void UsersModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules =
        [
            EventsNamespace,
            TicketingNamespace,
            AttendanceNamespace,
        ];

        string[] integrationEventsModules =
        [
            EventsIntegrationEventsNamespace,
            TicketingIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace,
        ];

        List<Assembly> usersAssemblies =
        [
            typeof(User).Assembly,
            Modules.Users.Application.AssemblyReference.Assembly,
            typeof(UsersModule).Assembly,
            Modules.Users.Presentation.AssemblyReference.Assembly,
        ];

        Types.InAssemblies(usersAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void TicketingModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules =
        [
            EventsNamespace,
            AttendanceNamespace,
            UsersNamespace,
        ];

        string[] integrationEventsModules =
        [
            EventsIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
        ];

        List<Assembly> ticketingAssemblies =
        [
            typeof(Ticket).Assembly,
            Modules.Ticketing.Application.AssemblyReference.Assembly,
            typeof(TicketingModule).Assembly,
            Modules.Ticketing.Presentation.AssemblyReference.Assembly,
        ];

        Types.InAssemblies(ticketingAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void EventsModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules =
        [
            TicketingNamespace,
            AttendanceNamespace,
            UsersNamespace,
        ];

        string[] integrationEventsModules =
        [
            TicketingIntegrationEventsNamespace,
            AttendanceIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
        ];

        List<Assembly> eventsAssemblies =
        [
            typeof(Event).Assembly,
            Modules.Events.Application.AssemblyReference.Assembly,
            typeof(EventsModule).Assembly,
            Modules.Events.Presentation.AssemblyReference.Assembly,
        ];

        Types.InAssemblies(eventsAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void AttendanceModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules =
        [
            TicketingNamespace,
            EventsNamespace,
            UsersNamespace,
        ];

        string[] integrationEventsModules =
        [
            TicketingIntegrationEventsNamespace,
            EventsIntegrationEventsNamespace,
            UsersIntegrationEventsNamespace,
        ];

        List<Assembly> attendanceAssemblies =
        [
            typeof(Attendee).Assembly,
            Modules.Attendance.Application.AssemblyReference.Assembly,
            typeof(AttendanceModule).Assembly,
            Modules.Attendance.Presentation.AssemblyReference.Assembly,
        ];

        Types.InAssemblies(attendanceAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();
    }
}
