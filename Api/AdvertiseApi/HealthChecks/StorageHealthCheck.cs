using AdvertiseApi.Services;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Threading;
using System.Threading.Tasks;

namespace AdvertiseApi.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertiseStorageService _storageService;

        public StorageHealthCheck(IAdvertiseStorageService service)
        {
            _storageService = service;
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var isStorageOk = await _storageService.CheckHealthAsync();
            return new HealthCheckResult(
                isStorageOk ? 
                HealthStatus.Healthy : 
                HealthStatus.Unhealthy
                );
        }
    }
}
