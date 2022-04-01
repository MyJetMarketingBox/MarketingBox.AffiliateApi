using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Grpc.Requests.Payout;
using MarketingBox.AffiliateApi.Models.Payouts;
using MarketingBox.AffiliateApi.Models.Payouts.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class PayoutMapperProfile:Profile
    {
        public PayoutMapperProfile()
        {
            CreateMap<PayoutUpsertRequest, PayoutCreateRequest>();
            CreateMap<PayoutUpsertRequest, PayoutUpdateRequest>();
            CreateMap<BrandPayout, BrandPayoutModel>();
            CreateMap<AffiliatePayout, AffiliatePayoutModel>();
        }
    }
}