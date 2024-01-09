namespace MetricsAgent.Jobs
{
    /// <summary>
    /// Хранит расписание выполняемой задачи
    /// </summary>
    public class JobSchedule
    {
        /// <summary>
        /// Выражение, описывающее расписание выполнения задачи
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Указатель на задачу
        /// </summary>
        public Type JobType { get; set; }

        public JobSchedule(Type jobType, string expression)
        {
            JobType = jobType;
            Expression = expression;
        }
    }
}
