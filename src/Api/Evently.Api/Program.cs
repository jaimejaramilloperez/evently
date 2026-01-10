using Evently.Api.Extensions;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Modules.Events.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly]);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEventsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok("0k"));

RouteGroupBuilder apiGroup = app.MapGroup("/api");

apiGroup.MapEventsModuleEndpoints();

app.Run();
