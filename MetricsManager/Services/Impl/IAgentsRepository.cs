using MetricsManager.Models;

namespace MetricsManager.Services.Impl
{
    public interface IAgentsRepository
    {
        int AddAgent(AgentInfo agentInfo);
        bool RemoveAgent(int id);
        List<AgentInfo> GetAll();
        AgentInfo GetAgentById(int id);
    }
}
