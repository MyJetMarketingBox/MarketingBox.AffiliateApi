using AutoMapper;
using MarketingBox.AffiliateApi.Models.Postback;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Domain.Models.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class PostbackReferenceProfile : Profile
    {
        public PostbackReferenceProfile()
        {
            CreateMap<ReferenceUpdateRequest, UpdateReferenceRequest>();
            CreateMap<ReferenceCreateRequest, CreateReferenceRequest>();
            CreateMap<Reference, MarketingBox.Postback.Service.Domain.Models.Reference>()
                .ReverseMap()
                .ForMember(
                    d => d.AffiliateName, 
                    m => 
                        m.MapFrom(s => s.Affiliate.Name));
        }
    }
}
