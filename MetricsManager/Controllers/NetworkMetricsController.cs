using MetricsAgentNamespace;
using MetricsManager.Models;
using MetricsManager.Services.Impl;
using MetricsManager.Services.Impl.Clients;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly IMetricsAgentDefaultClient _metricsAgentDefaultClient;

        public NetworkMetricsController(IMetricsAgentDefaultClient metricsAgentDefaultClient)
        {
            _metricsAgentDefaultClient = metricsAgentDefaultClient;
        }


        [SwaggerOperation("GetNetworkMetrics")]
        [HttpGet("agent/{agentId}/from/{fromDate}/to/{toDate}", Name = "GetNetworkMetrics")]
        public ActionResult<ICollection<NetworkMetricDto>> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            try
            {
                return Ok(_metricsAgentDefaultClient.Build(agentId).GetNetworkMetricsAsync(fromDate, toDate).Result);
            }
            catch
            {
                return Ok(new List<NetworkMetricDto>());
            }
        }         
    }
}

