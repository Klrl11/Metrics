using MetricsAgent.Services;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private PerformanceCounter _performanceCounter;

        public RamMetricJob(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _performanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
        }
        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var ramMetricsRepository = scope.ServiceProvider.GetService<IRamMetricsRepository>();
            var ramUsageInPersents = _performanceCounter.NextValue();
            ramMetricsRepository.Add(new Models.RamMetric
            {
                Time = DateTime.Now,
                Value = (int)ramUsageInPersents
            });

            //TODO: Выполнение задачи ...
            Debug.WriteLine($"RamMetricJob >>> {DateTime.Now}");
            // new PerformanceCounter("Memory", "% Committed Bytes In Use");
            return Task.CompletedTask;
        }        
    }
}
