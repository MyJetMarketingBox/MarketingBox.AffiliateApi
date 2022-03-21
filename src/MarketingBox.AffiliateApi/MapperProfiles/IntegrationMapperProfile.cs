using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Integrations;
using MarketingBox.AffiliateApi.Models.Integrations;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class IntegrationMapperProfile : Profile
    {
        public IntegrationMapperProfile()
        {
            CreateMap<Integration, IntegrationModel>();
        }
    }
}