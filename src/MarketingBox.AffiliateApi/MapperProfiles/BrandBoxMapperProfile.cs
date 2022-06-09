using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.BrandBox;
using MarketingBox.Affiliate.Service.Grpc.Requests.BrandBox;
using MarketingBox.AffiliateApi.Models.BrandBox;
using MarketingBox.AffiliateApi.Models.BrandBox.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class BrandBoxMapperProfile : Profile
    {
        public BrandBoxMapperProfile()
        {
            CreateMap<BrandBox, BrandBoxModel>();
            CreateMap<BrandBoxUpsertRequest, BrandBoxCreateRequest>();
            CreateMap<BrandBoxUpsertRequest, BrandBoxUpdateRequest>();
        }
    }
}