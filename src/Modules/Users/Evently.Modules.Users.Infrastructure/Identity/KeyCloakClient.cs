using System.Net.Http.Json;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
    internal async Task<Guid> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            new Uri("users", UriKind.Relative),
            user,
            cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static Guid ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        Uri? locationHeader = httpResponseMessage.Headers.Location;

        if (locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        string identityId = locationHeader.PathAndQuery.Split("/")[^1];

        if (!Guid.TryParse(identityId, out Guid parsedIdentityId))
        {
            throw new InvalidOperationException("Identity Id is not a guid");
        }

        return parsedIdentityId;
    }
}
