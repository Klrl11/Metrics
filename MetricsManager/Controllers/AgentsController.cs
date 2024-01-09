using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Models.Requests;
using MetricsManager.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMapper _mapper;

        public AgentsController(
            IAgentsRepository agentsRepository,
            IMapper mapper)
        {
            _agentsRepository = agentsRepository;
            _mapper = mapper;
        }


        [SwaggerOperation("RegisterAgent")]
        [HttpPost("register", Name = "RegisterAgent")]
        public ActionResult<int> RegisterAgent([FromBody] AgentInfoRequest AgentInfoRequest)
        {
            return Ok(_agentsRepository.AddAgent(_mapper.Map<AgentInfo>(AgentInfoRequest)));
        }

        [SwaggerOperation("GetAllAgents")]
        [HttpGet("getAll", Name = "GetAllAgents")]
        public ActionResult<List<AgentInfo>> GetAll()
        {
            return Ok(_agentsRepository.GetAll());
        }

        [SwaggerOperation("GetAgentById")]
        [HttpGet("getById", Name = "GetAgentById")]
        public ActionResult<AgentInfo> GetById([FromQuery] int id)
        {
            return Ok(_agentsRepository.GetAgentById(id));
        }

        [SwaggerOperation("DeleteAgentById")]
        [HttpDelete("deleteById", Name = "DeleteAgentById")]
        public ActionResult<bool> DeleteById([FromBody] int id)
        {
            return Ok(_agentsRepository.RemoveAgent(id));
        }
    }
}
