using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.Dto;
using MetricsAgent.Models.Requests;

namespace MetricsAgent.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<CpuMetricCreateRequest, CpuMetric>();
            CreateMap<CpuMetric, CpuMetricDto>();

            CreateMap<HddMetricCreateRequest, HddMetric>();
            CreateMap<HddMetric, HddMetricDto>();

            CreateMap<NetworkMetricCreateRequest, NetworkMetric>();
            CreateMap<NetworkMetric, NetworkMetricDto>();

            CreateMap<RamMetricCreateRequest, RamMetric>();
            CreateMap<RamMetric, RamMetricDto>();

        }
    }
}
