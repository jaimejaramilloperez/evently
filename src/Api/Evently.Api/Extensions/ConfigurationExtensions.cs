namespace Evently.Api.Extensions;

internal static class ConfigurationExtensions
{
    internal static void AddModuleConfiguration(
        this IConfigurationBuilder configurationBuilder,
        string[] modules)
    {
        foreach (string module in modules)
        {
            configurationBuilder.AddJsonFile(
                path: $"modules.{module}.json",
                optional: false,
                reloadOnChange: true);

            configurationBuilder.AddJsonFile(
                path: $"modules.{module}.Development.json",
                optional: true,
                reloadOnChange: true);
        }

        configurationBuilder.AddEnvironmentVariables();
    }
}
