using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace shared.api.Extensions;

public static class PollyExtensions
{
    private static AsyncRetryPolicy<HttpResponseMessage> PolicyWaitAndRetry(params int[] sleepDurations)
    {
        var sleepDurationTimeSpan = sleepDurations.Select(sleepDuration => TimeSpan.FromMilliseconds(sleepDuration));

        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(sleepDurationTimeSpan);

        return retryPolicy;
    }

    public static AsyncRetryPolicy<HttpResponseMessage> WaitAndRetry()
    {
        return PolicyWaitAndRetry(1000, 2000);
    }
}