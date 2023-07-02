using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pros.Service
{
    public class CustomHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            return HealthCheckResult.Healthy();
        }
    }
}
