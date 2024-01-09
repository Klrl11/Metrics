using Dapper;
using MetricsManager.Models.Options;
using MetricsManager.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace MetricsManager.Services.Impl
{
    public class AgentsRepository : IAgentsRepository
    {
        public ConnectionStrings ConnectionOptions { get; }

        public AgentsRepository( 
            IOptions<ConnectionStrings> connectionOptions)
        {
            ConnectionOptions = connectionOptions.Value;
        }

        public int AddAgent(AgentInfo agentInfo)
        {
            try
            {
                using SqliteConnection connection = new(ConnectionOptions.Default);
                int res = connection.Execute("INSERT INTO AgentsTable(address) VALUES (@address)",
                    new
                    {
                        address = agentInfo.Address,
                    });
                return res;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public AgentInfo? GetAgentById(int agentId)
        {
            using var connection = new SqliteConnection(ConnectionOptions.Default);
            return connection.Query<AgentInfo>("SELECT id, address FROM AgentsTable WHERE id == @idNum",
                new
                {
                    idNum = agentId
                }).FirstOrDefault();
        }

        public List<AgentInfo> GetAll()
        {
            using var connection = new SqliteConnection(ConnectionOptions.Default);
            return connection.Query<AgentInfo>("SELECT id, address FROM AgentsTable").ToList<AgentInfo>();
        }

        public bool RemoveAgent(int idNumber)
        {
            try
            {
                using var connection = new SqliteConnection(ConnectionOptions.Default);
                int res = connection.Execute("DELETE FROM AgentsTable WHERE id = @idNum",
                    new
                    {
                        idNum = idNumber
                    });
                return res >= 1;
                
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
