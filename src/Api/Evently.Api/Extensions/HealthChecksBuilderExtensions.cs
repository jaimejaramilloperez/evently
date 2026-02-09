namespace Evently.Api.Extensions;

internal static class KeyCloakHealthChecksBuilderExtensions
{
    internal static IHealthChecksBuilder AddKeyCloak(this IHealthChecksBuilder builder, string url)
    {
        builder.AddUrlGroup(new Uri(url), HttpMethod.Get, "KeyCloak");

        return builder;
    }
}
