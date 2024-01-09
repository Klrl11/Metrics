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
    public class HddMetricsController : ControllerBase
    {

        private IHddMetricsRepository _hddMetricsRepository;
        private IMapper _mapper;

        public HddMetricsController(
            IMapper mapper,
            IHddMetricsRepository hddMetricsRepository)
        {
            _hddMetricsRepository = hddMetricsRepository;
            _mapper = mapper;
        }

                
        [SwaggerOperation("HddMetricsGetByDate")]
        [HttpGet("getbydate", Name = "HddMetricsGetByDate")]
        public ActionResult<List<HddMetricDto>> GetByDate(DateTime fromDate, DateTime toDate) =>
            Ok(_mapper.Map<List<HddMetricDto>>(_hddMetricsRepository.GetByDate(fromDate, toDate)));

    }
}
