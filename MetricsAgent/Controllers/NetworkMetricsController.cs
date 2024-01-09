using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using MetricsAgent.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {

        private INetworkMetricsRepository _networkMetricsRepository; 
        private IMapper _mapper;


        public NetworkMetricsController(
            IMapper mapper,
            INetworkMetricsRepository networkMetricsRepository)
        {
            _networkMetricsRepository = networkMetricsRepository;
            _mapper = mapper;
        }

        [SwaggerOperation("NetworkMetricsGetByDate")]
        [HttpGet("getbydate", Name = "NetworkMetricsGetByDate")]
        public ActionResult<List<NetworkMetricDto>> GetByDate(DateTime fromDate, DateTime toDate) =>
            Ok(_mapper.Map<List<NetworkMetricDto>>(_networkMetricsRepository.GetByDate(fromDate, toDate)));

    }
}
