using MetricsAgentNamespace;
using MetricsManager.Models;
using MetricsManager.Services.Impl;
using MetricsManager.Services.Impl.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly IMetricsAgentDefaultClient _metricsAgentDefaultClient;

        public CpuMetricsController(IMetricsAgentDefaultClient metricsAgentDefaultClient)
        {
            _metricsAgentDefaultClient = metricsAgentDefaultClient;
        }


        [SwaggerOperation("GetCpuMetrics")]
        [HttpGet("agent/{agentId}/from/{fromDate}/to/{toDate}", Name = "GetCpuMetrics")]
        public ActionResult<ICollection<CpuMetricDto>> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            try
            {
                return Ok(_metricsAgentDefaultClient.Build(agentId).GetCpuMetricsAsync(fromDate, toDate).Result);
            }
            catch
            {
                return Ok(new List<CpuMetricDto>());
            }
        }
    }
}
