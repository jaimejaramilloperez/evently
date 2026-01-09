namespace Evently.Modules.Events.Application.Abstractions.Exceptions;

public sealed class EventlyException : Exception
{
    public EventlyException()
        : base("An application error occurred.")
    {
    }

    public EventlyException(string message)
        : base(message)
    {
    }

    public EventlyException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
