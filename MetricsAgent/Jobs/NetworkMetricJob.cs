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
            // Получить список доступных сетевых интерфейсов
            var performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            // Получить список наименований доступных сетевых интерфейсов
            var instanceNames = performanceCounterCategory.GetInstanceNames();
            if (instanceNames != null && instanceNames.Length > 0)
                // Получим статистику по первому доступному, активному сетевому интерфейсу
                _performanceCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instanceNames[0]);
        }
        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var networkMetricsRepository = scope.ServiceProvider.GetService<INetworkMetricsRepository>();
            // Если существует досупное, активное сетевое устройство, получим
            // статистику по его использованию, сохраним в бд
            if (_performanceCounter != null)
            {
            var networkUsageInPersents = _performanceCounter.NextValue();
            networkMetricsRepository.Add(new Models.NetworkMetric
            {
                Time = DateTime.Now,
                Value = (int)networkUsageInPersents
            });
            }

            Debug.WriteLine($"NetworkMetricJob >>> {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
