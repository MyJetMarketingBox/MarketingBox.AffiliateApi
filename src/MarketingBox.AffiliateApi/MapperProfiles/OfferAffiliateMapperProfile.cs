using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.OfferAffiliates;
using MarketingBox.Affiliate.Service.Grpc.Requests.OfferAffiliate;
using MarketingBox.AffiliateApi.Models.OfferAffiliates;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class OfferAffiliateMapperProfile:Profile
    {
        public OfferAffiliateMapperProfile()
        {
            CreateMap<OfferAffiliateUpsertRequest, OfferAffiliateCreateRequest>();
            CreateMap<OfferAffiliate, OfferAffiliateModel>();
        }
    }
}