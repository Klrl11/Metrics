using MetricsAgentNamespace;

namespace MetricsManager.Services.Impl.Clients
{
    public interface IMetricsAgentDefaultClient
    {
        IMetricsAgentDefaultClient Build(int agentId);

        Task<ICollection<CpuMetricDto>> GetCpuMetricsAsync(DateTime from, DateTime to);

        Task<ICollection<HddMetricDto>> GetHddMetricsAsync(DateTime from, DateTime to);

        Task<ICollection<NetworkMetricDto>> GetNetworkMetricsAsync(DateTime from, DateTime to);

        Task<ICollection<RamMetricDto>> GetRamMetricsAsync(DateTime from, DateTime to);

    }
}
