using AutoMapper;
using MarketingBox.AffiliateApi.Models.Postback;
using MarketingBox.AffiliateApi.Models.Postback.Requests;
using MarketingBox.Postback.Service.Grpc.Models;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class PostbackReferenceProfile : Profile
    {
        public PostbackReferenceProfile()
        {
            CreateMap<ReferenceRequest, FullReferenceRequest>()
                .ForMember(
                    x => x.AffiliateId,
                    o => o.MapFrom(p => p.AffiliateId));
            CreateMap<ReferenceResponse, Reference>();
        }
    }
}
