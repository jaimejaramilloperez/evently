using System.Net;

namespace Evently.IntegrationTests.Abstractions;

internal static class Poller
{
    internal static async Task<HttpResponseMessage> WaitAsync(
        TimeSpan timeout,
        Func<Task<HttpResponseMessage>> func,
        CancellationToken cancellationToken = default)
    {
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(2));

        DateTime endTimeUtc = DateTime.UtcNow.Add(timeout);

        while (DateTime.UtcNow < endTimeUtc && await timer.WaitForNextTickAsync(cancellationToken))
        {
            HttpResponseMessage result = await func();

            if (result.IsSuccessStatusCode)
            {
                return result;
            }
        }

        return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
    }
}
