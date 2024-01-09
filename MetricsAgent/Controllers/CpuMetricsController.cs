using AutoMapper;
using FluentMigrator.Runner;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;
using MetricsAgent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {

        private ICpuMetricsRepository _cpuMetricsRepository;
        private IMapper _mapper;

        public CpuMetricsController(
            IMapper mapper,
            ICpuMetricsRepository cpuMetricsRepository)
        {
            _cpuMetricsRepository = cpuMetricsRepository;
            _mapper = mapper;
        }

        [SwaggerOperation("CpuMetricsGetByDate")]
        [HttpGet("getbydate", Name = "CpuMetricsGetByDate")]
        public ActionResult<List<CpuMetricDto>> GetByDate(DateTime fromDate, DateTime toDate)
        {           
           return Ok(_mapper.Map<List<CpuMetricDto>>(_cpuMetricsRepository.GetByDate(fromDate, toDate)));
        }
    }
}
