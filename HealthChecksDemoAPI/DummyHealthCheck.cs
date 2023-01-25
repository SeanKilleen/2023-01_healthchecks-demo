using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecksDemoAPI
{
    public class DummyHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(HealthCheckResult.Healthy("The dummy health check is working correctly"));
        }
    }
}
