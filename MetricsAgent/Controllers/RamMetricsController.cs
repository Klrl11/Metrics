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
    public class RamMetricsController : ControllerBase
    {

        private IRamMetricsRepository _ramMetricsRepository;
        private IMapper _mapper;

        public RamMetricsController(
            IMapper mapper,
            IRamMetricsRepository ramMetricsRepository)
        {
            _ramMetricsRepository = ramMetricsRepository;
            _mapper = mapper;
        }

        [SwaggerOperation("RamMetricsGetByDate")]
        [HttpGet("getbydate", Name = "RamMetricsGetByDate")]
        public ActionResult<List<RamMetricDto>> GetByDate(DateTime fromDate, DateTime toDate) =>
            Ok(_mapper.Map<List<RamMetricDto>>(_ramMetricsRepository.GetByDate(fromDate, toDate)));

    }
}
