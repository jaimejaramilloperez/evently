using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    internal static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddJwtBearer();

        services.AddAuthorization();

        services.ConfigureOptions<JwtBearerConfigureOptions>();

        return services;
    }
}
