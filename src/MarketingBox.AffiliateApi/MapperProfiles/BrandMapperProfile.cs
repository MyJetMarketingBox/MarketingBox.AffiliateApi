using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.AffiliateApi.Models.Brands;
using MarketingBox.AffiliateApi.Models.Brands.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class BrandMapperProfile :Profile
    {
        public BrandMapperProfile()
        {
            CreateMap<BrandUpsertRequest, Affiliate.Service.Grpc.Requests.Brands.BrandCreateRequest>();
            CreateMap<BrandUpsertRequest, Affiliate.Service.Grpc.Requests.Brands.BrandUpdateRequest>();
            CreateMap<Brand, BrandModel>();
        }
    }
}