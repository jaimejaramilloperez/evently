using System.Diagnostics;
using Serilog.Context;

namespace Evently.Api.Middlewares;

internal sealed class LogContextTraceLoggingMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext httpContext)
    {
        string? traceId = Activity.Current?.TraceId.ToString();

        using (LogContext.PushProperty("TraceId", traceId))
        {
            return next(httpContext);
        }
    }
}
