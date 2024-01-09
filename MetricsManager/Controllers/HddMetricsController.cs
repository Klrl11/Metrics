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
    public class HddMetricsController : ControllerBase
    {

        private readonly IMetricsAgentDefaultClient _metricsAgentDefaultClient;


        public HddMetricsController(IMetricsAgentDefaultClient metricsAgentDefaultClient)
        {
            _metricsAgentDefaultClient = metricsAgentDefaultClient;
        }

        [SwaggerOperation("GetHddMetrics")]
        [HttpGet("agent/{agentId}/from/{fromDate}/to/{toDate}", Name = "GetHddMetrics")]        
        public ActionResult<ICollection<HddMetricDto>> GetMetricsFromAgent(
            [FromRoute] int agentId, [FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            try
            {
                return Ok(_metricsAgentDefaultClient.Build(agentId).GetHddMetricsAsync(fromDate, toDate).Result);
            }
            catch
            {
                return Ok(new List<HddMetricDto>());
            }
        }
    }
}
