using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private PerformanceCounter _performanceCounter;

        public HddMetricJob(
           IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var performanceCounterCategory = new PerformanceCounterCategory("PhysicalDisk");
            var instanceNames = performanceCounterCategory.GetInstanceNames();
            _performanceCounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", instanceNames.FirstOrDefault(item => item.Contains("C:")));
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var hddMetricsRepository = scope.ServiceProvider.GetService<IHddMetricsRepository>();
            var hddUsageInPercents = _performanceCounter.NextValue();
            hddMetricsRepository.Add(new Models.HddMetric
            {
                Time = DateTime.Now,
                Value = (int)hddUsageInPercents
            });
            Debug.WriteLine($"HddMetricJob >>> {DateTime.Now}: {hddUsageInPercents:F2}");
            return Task.CompletedTask;
        }        
    }
}