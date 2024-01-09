using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private PerformanceCounter _performanceCounter;

        public NetworkMetricJob(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _performanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", "Intel[R] Wi-Fi 6 AX201 160MHz");
        }
        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var networkMetricsRepository = scope.ServiceProvider.GetService<INetworkMetricsRepository>();
            var networkUsageInPersents = _performanceCounter.NextValue();
            networkMetricsRepository.Add(new Models.NetworkMetric
            {
                Time = DateTime.Now,
                Value = (int)networkUsageInPersents
            });

            //TODO: Выполнение задачи ...
            Debug.WriteLine($"NetworkMetricJob >>> {DateTime.Now}");
            //new PerformanceCounter("Network Interface", "Bytes Total/sec", "Intel[R] Wi-Fi 6 AX201 160MHz");
            return Task.CompletedTask;
        }
    }
}
