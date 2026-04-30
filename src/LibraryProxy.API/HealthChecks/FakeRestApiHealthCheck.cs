using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryProxy.API.HealthChecks;

public class FakeRestApiHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FakeRestApiHealthCheck(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("FakeRestAPI");
            var response = await client.GetAsync("Books", cancellationToken);

            if (response.IsSuccessStatusCode)
                return HealthCheckResult.Healthy("FakeRestAPI is reachable and responding.");

            return HealthCheckResult.Unhealthy($"FakeRestAPI returned HTTP {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("FakeRestAPI is unreachable.", ex);
        }
    }
}
