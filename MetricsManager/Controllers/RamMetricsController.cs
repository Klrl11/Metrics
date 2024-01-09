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
    public class RamMetricsController : ControllerBase
    {
        private readonly IMetricsAgentDefaultClient _metricsAgentDefaultClient;

        public RamMetricsController(IMetricsAgentDefaultClient metricsAgentDefaultClient)
        {
            _metricsAgentDefaultClient = metricsAgentDefaultClient;
        }


        [SwaggerOperation("GetRamMetrics")]
        [HttpGet("agent/{agentId}/from/{fromDate}/to/{toDate}", Name = "GetRamMetrics")]
        public ActionResult<ICollection<RamMetricDto>> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            try
            {
                return Ok(_metricsAgentDefaultClient.Build(agentId).GetRamMetricsAsync(fromDate, toDate).Result);
            }
            catch
            {
                return Ok(new List<RamMetricDto>());
            }
        }
    }
}
