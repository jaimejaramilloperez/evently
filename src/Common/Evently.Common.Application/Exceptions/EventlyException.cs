using Evently.Common.Domain.Results;

namespace Evently.Common.Application.Exceptions;

public sealed class EventlyException : Exception
{
    public const string DefaultErrorMessage = "An application error occurred.";

    public string? RequestName { get; }
    public Error? Error { get; }

    public EventlyException()
        : base(DefaultErrorMessage)
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

    public EventlyException(string requestName, Error error, Exception? innerException = default)
        : base(DefaultErrorMessage, innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public EventlyException(string message, string requestName, Error? error = default, Exception? innerException = default)
        : base(message, innerException)
    {
        RequestName = requestName;
        Error = error;
    }
}
