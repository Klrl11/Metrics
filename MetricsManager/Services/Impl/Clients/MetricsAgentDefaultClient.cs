using MetricsAgentNamespace;

namespace MetricsManager.Services.Impl.Clients
{
    public class MetricsAgentDefaultClient : IMetricsAgentDefaultClient
    {

        private readonly IAgentsRepository _agentsRepository;
        private readonly HttpClient _httpClient;
        private MetricsAgentClient _metricsAgentClient;

        public MetricsAgentDefaultClient(
            IAgentsRepository agentsRepository,
            HttpClient httpClient)
        {
            _agentsRepository = agentsRepository;
            _httpClient = httpClient;
        }

        public IMetricsAgentDefaultClient Build(int agentId)
        {
            var agentInfo = _agentsRepository.GetAgentById(agentId);
            if (agentInfo == null)
            {
                throw new Exception("Идентификатор агента указан некорректно.");
            }
            _metricsAgentClient = new MetricsAgentClient(agentInfo.Address, _httpClient);
            return this;
        }

        public async Task<ICollection<CpuMetricDto>> GetCpuMetricsAsync(DateTime from, DateTime to)
        {
            return await _metricsAgentClient.CpuMetricsGetByDateAsync(from, to);
        }

        public async Task<ICollection<HddMetricDto>> GetHddMetricsAsync(DateTime from, DateTime to)
        {
            return await _metricsAgentClient.HddMetricsGetByDateAsync(from, to);
        }

        public async Task<ICollection<NetworkMetricDto>> GetNetworkMetricsAsync(DateTime from, DateTime to)
        {
            return await _metricsAgentClient.NetworkMetricsGetByDateAsync(from, to);
        }

        public async Task<ICollection<RamMetricDto>> GetRamMetricsAsync(DateTime from, DateTime to)
        {
            return await _metricsAgentClient.RamMetricsGetByDateAsync(from, to);
        }
    }
}
