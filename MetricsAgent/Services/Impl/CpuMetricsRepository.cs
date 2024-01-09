using Dapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Options;
using MetricsAgent.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace MetricsAgent.Services.Impl
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    { 
        public ServiceSettings ServiceSettings { get; }
        public ConnectionStrings Connections { get; }

        public CpuMetricsRepository(
            IOptions<ServiceSettings> serviceSettingsOptions,
            IOptions<ConnectionStrings> connectionStringsOptions)
        {
            ServiceSettings = serviceSettingsOptions.Value;
            Connections = connectionStringsOptions.Value;
        }

        public int Add(CpuMetric item)
        {
            try
            {
                using SqliteConnection connection = new(Connections.Default);
                int res = connection.Execute("INSERT INTO cpumetrics(value, time) VALUES (@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.Ticks
                    });
                return res;
            }
            catch (Exception ex) {
                return 0;
            }            
        }

        public IList<CpuMetric> GetByDate(DateTime dateFrom, DateTime dateTo)
        {
            using var connection = new SqliteConnection(Connections.Default);
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            return connection.Query<CpuMetric>("SELECT id, value, time FROM cpumetrics WHERE time >= @timeFrom and time <= @timeTo",
                new
                {
                    timeFrom = dateFrom.Ticks,
                    timeTo = dateTo.Ticks
                }).ToList();
        }
    }
}
