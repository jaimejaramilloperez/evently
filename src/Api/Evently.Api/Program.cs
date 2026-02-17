using System.Diagnostics;
using Evently.Api.Extensions;
using Evently.Api.Middlewares;
using Evently.Api.OpenApi;
using Evently.Api.OpenTelemetry;
using Evently.Api.Serialization;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Infrastructure.Configuration;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Http.Features;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureOptions<ConfigureJsonOptions>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

    Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
    context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication([
    Evently.Modules.Events.Application.AssemblyReference.Assembly,
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
    Evently.Modules.Attendance.Application.AssemblyReference.Assembly,
]);

builder.Services.AddInfrastructure(DiagnosticsConfig.ServiceName, builder.Configuration, [
    EventsModule.ConfigureConsumers(builder.Configuration.GetConnectionStringOrThrow("Cache")),
    TicketingModule.ConfigureConsumers,
    AttendanceModule.ConfigureConsumers,
]);

builder.Configuration.AddModuleConfiguration(["events", "users", "ticketing", "attendance"]);

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);
builder.Services.AddAttendanceModule(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionStringOrThrow("Database"))
    .AddRedis(builder.Configuration.GetConnectionStringOrThrow("Cache"))
    .AddKeyCloak(builder.Configuration.GetValueOrThrow<string>("KeyCloak:HealthUrl"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Evently Api - Swagger Docs";
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
        options.DisplayRequestDuration();
    });

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseLogContextTraceLogging();

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapHealthChecks("/health", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponseNoExceptionDetails,
});

RouteGroupBuilder apiGroup = app.MapGroup("/api");

app.MapEndpoints(apiGroup);

app.Run();
