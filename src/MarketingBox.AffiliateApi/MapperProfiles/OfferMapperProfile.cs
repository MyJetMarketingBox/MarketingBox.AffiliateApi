using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Affiliate.Service.Grpc.Requests.Offers;
using MarketingBox.AffiliateApi.Models.Offers;
using MarketingBox.AffiliateApi.Models.Offers.Requests;
using OfferCreateRequestAPI = MarketingBox.AffiliateApi.Models.Offers.Requests.OfferCreateRequest;
using OfferCreateRequestGRPC = MarketingBox.Affiliate.Service.Grpc.Requests.Offers.OfferCreateRequest;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class OfferMapperProfile : Profile
    {
        public OfferMapperProfile()
        {
            CreateMap<OfferCreateRequestAPI, OfferCreateRequestGRPC>();
            CreateMap<OfferSubParameterRequest, OfferSubParameterCreateRequest>();

            CreateMap<Offer, OfferModel>();
            CreateMap<OfferSubParameter, OfferSubParameterModel>();
        }
    }
}