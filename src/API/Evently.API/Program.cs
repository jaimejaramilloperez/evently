using Evently.API.Extensions;
using Evently.Modules.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok("0K"));

RouteGroupBuilder apiGroup = app.MapGroup("/api");

apiGroup.MapEventsModuleEndpoints();

app.Run();
