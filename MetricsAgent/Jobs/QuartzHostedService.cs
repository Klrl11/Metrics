using Quartz;
using Quartz.Spi;

namespace MetricsAgent.Jobs
{
    public class QuartzHostedService : IHostedService
    {

        /// <summary>
        /// Создает новый джоб
        /// </summary>
        private readonly IJobFactory _jobFactory;

        /// <summary>
        /// Коллекция сожержит настройки по каждому джобу, теперь нам понятно
        /// с какими настройками нам запускать каждый джоб
        /// </summary>
        private readonly IEnumerable<JobSchedule> _jobSchedules;

        /// <summary>
        /// Фабрика создает объект IScheduler,
        /// позволяющий запускать на выполнение/останов работу нашего джоба
        /// </summary>
        private readonly ISchedulerFactory _schedulerFactory;


        public QuartzHostedService(
            IJobFactory jobFactory,
            ISchedulerFactory schedulerFactory,
            IEnumerable<JobSchedule> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        public IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (JobSchedule schedule in _jobSchedules)
            {
                await Scheduler.ScheduleJob(CreateJobDetail(schedule), CreateTrigger(schedule), cancellationToken);
            }
            await Scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (Scheduler != null)
            {
                await Scheduler.Shutdown(cancellationToken);
            }
        }

        /// <summary>
        /// Содержит информацию о типе задачи
        /// </summary>
        /// <returns></returns>
        private static IJobDetail CreateJobDetail(JobSchedule jobSchedule)
        {
            IJobDetail jobDetail = JobBuilder
                .Create(jobSchedule.JobType)
                .Build();
            return jobDetail;
        }

        /// <summary>
        /// Содержит информацию о времени запуска вашей задачи
        /// </summary>
        /// <returns></returns>
        private static ITrigger CreateTrigger(JobSchedule jobSchedule)
        {
            // https://www.freeformatter.com/cron-expression-generator-quartz.html
            ITrigger jobTrigger = TriggerBuilder
                .Create()
                .WithCronSchedule(jobSchedule.Expression)
                .Build();
            return jobTrigger;
        }

    }
}
