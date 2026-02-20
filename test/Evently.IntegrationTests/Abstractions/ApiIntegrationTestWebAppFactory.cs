using Evently.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Evently.IntegrationTests.Abstractions;

public class ApiIntegrationTestWebAppFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public async ValueTask InitializeAsync()
    {
        await SharedTestInfrastructure.StartAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await SharedTestInfrastructure.StopAsync();
        GC.SuppressFinalize(this);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Database", SharedTestInfrastructure.Database.GetConnectionString());
        builder.UseSetting("ConnectionStrings:Cache", SharedTestInfrastructure.Redis.GetConnectionString());
        builder.UseSetting("ConnectionStrings:Queue", SharedTestInfrastructure.RabbitMq.GetConnectionString());

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            string keyCloakAddress = SharedTestInfrastructure.Keycloak.GetBaseAddress();
            string keyCloakRealmUrl = $"{keyCloakAddress}realms/evently";

            Dictionary<string, string?> configurationOverrides = new()
            {
                ["Authentication:TokenValidationParameters:ValidIssuers:1"] = keyCloakRealmUrl,
                ["Authentication:MetadataAddress"] = $"{keyCloakAddress}.well-known/openid-configuration",
                ["Users:KeyCloak:AdminUrl"] = $"{keyCloakAddress}admin/realms/evently/",
                ["Users:KeyCloak:TokenUrl"] = $"{keyCloakRealmUrl}/protocol/openid-connect/token",
                ["Users:Outbox:IntervalInSeconds"] = "5",
                ["Users:Inbox:IntervalInSeconds"] = "5",
                ["Events:Outbox:IntervalInSeconds"] = "5",
                ["Events:Inbox:IntervalInSeconds"] = "5",
                ["Attendance:Outbox:IntervalInSeconds"] = "5",
                ["Attendance:Inbox:IntervalInSeconds"] = "5",
            };

            configBuilder.AddInMemoryCollection(configurationOverrides);
        });

        builder.ConfigureServices(services =>
        {
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = null;
                options.MetadataAddress = null!;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (token, parameters) => new JsonWebToken(token)
                };
            });
        });

        Quartz.Logging.LogContext.SetCurrentLogProvider(NullLoggerFactory.Instance);
    }
}
