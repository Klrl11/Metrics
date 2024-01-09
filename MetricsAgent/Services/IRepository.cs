namespace MetricsAgent.Services
{
    public interface IRepository<T>
    {
        int Add(T item);

        IList<T> GetByDate(DateTime dateFrom, DateTime dateTo);

    }
}
