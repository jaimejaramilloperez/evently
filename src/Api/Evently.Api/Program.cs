using System.Diagnostics;
using Evently.Api.Extensions;
using Evently.Api.Middlewares;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Http.Features;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenApi();

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
]);

builder.Services.AddInfrastructure(builder.Configuration, [
    TicketingModule.ConfigureConsumers,
]);

builder.Configuration.AddModuleConfiguration(["events", "users", "ticketing"]);

builder.Services.AddEventsModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddTicketingModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.MapHealthChecks("/health", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponseNoExceptionDetails,
});

RouteGroupBuilder apiGroup = app.MapGroup("/api");

app.MapEndpoints(apiGroup);

app.Run();
