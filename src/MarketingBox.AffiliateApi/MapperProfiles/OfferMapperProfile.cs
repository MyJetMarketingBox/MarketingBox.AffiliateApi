using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.AffiliateApi.Models.Offers;
using MarketingBox.AffiliateApi.Models.Offers.Requests;
using OfferCreateRequestGRPC = MarketingBox.Affiliate.Service.Grpc.Requests.Offers.OfferCreateRequest;
using OfferUpdateRequestGRPC = MarketingBox.Affiliate.Service.Grpc.Requests.Offers.OfferUpdateRequest;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class OfferMapperProfile : Profile
    {
        public OfferMapperProfile()
        {
            CreateMap<OfferUpsertRequest, OfferCreateRequestGRPC>();
            CreateMap<OfferUpsertRequest, OfferUpdateRequestGRPC>();
            CreateMap<Offer, OfferModel>();
        }
    }
}