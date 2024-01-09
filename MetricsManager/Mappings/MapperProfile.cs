using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Models.Requests;

namespace MetricsManager.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<AgentInfoRequest, AgentInfo>();

        }
    }
}
