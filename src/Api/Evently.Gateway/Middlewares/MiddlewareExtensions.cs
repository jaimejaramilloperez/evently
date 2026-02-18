namespace Evently.Gateway.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseLogContextTraceLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogContextTraceLoggingMiddleware>();

        return app;
    }
}
