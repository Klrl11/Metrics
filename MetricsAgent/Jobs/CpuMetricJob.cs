using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class CpuMetricJob : IJob
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private PerformanceCounter _performanceCounter;

        public CpuMetricJob(
           IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var cpuMetricsRepository = scope.ServiceProvider.GetService<ICpuMetricsRepository>();
            var cpuUsageInPercents = _performanceCounter.NextValue();
            cpuMetricsRepository.Add(new Models.CpuMetric
            {
                Time = DateTime.Now,
                Value = (int)cpuUsageInPercents
            });
            Debug.WriteLine($"CpuMetricJob >>> {DateTime.Now}: {cpuUsageInPercents:F2}");
            return Task.CompletedTask;
        }
    }
}
