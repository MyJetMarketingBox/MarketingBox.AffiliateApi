using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Affiliate.Service.Grpc.Requests.Offers;
using MarketingBox.AffiliateApi.Models.Offers;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class OfferMapperProfile:Profile
    {
        public OfferMapperProfile()
        {
            CreateMap<Offer, OfferModel>();
            CreateMap<OfferSubParameter, OfferSubParameterModel>();
            CreateMap<OfferCreateRequest, OfferCreateRequest>();
        }
    }
}