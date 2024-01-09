using MetricsAgent.Services;
using Quartz;
using Quartz.Spi;

namespace MetricsAgent.Jobs
{
    /// <summary>
    /// Фабрика по созданию новых объектов-задач
    /// </summary>
    public class SingletonJobFactory : IJobFactory
    {

        private readonly IServiceProvider _serviceProvider;

        public SingletonJobFactory(IServiceProvider serviceProvider)
        { 
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {   
            return (IJob)_serviceProvider.GetRequiredService(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            //TODO: Пока мне это не очень интересно ...
        }
    }
}
