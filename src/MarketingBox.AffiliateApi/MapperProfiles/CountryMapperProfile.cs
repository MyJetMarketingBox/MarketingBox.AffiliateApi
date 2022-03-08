using AutoMapper;
using MarketingBox.AffiliateApi.Models.Country;
using GrpcModel = MarketingBox.Affiliate.Service.Domain.Models.Country;
using ApiModel = MarketingBox.AffiliateApi.Models.Country.Requests;
using GrpcRequestModel = MarketingBox.Affiliate.Service.Grpc.Models.Country;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class CountryMapperProfile : Profile
    {
        public CountryMapperProfile()
        {
            CreateMap<GrpcModel.Country, Country>();
            CreateMap<GrpcModel.Geo, Geo>();
            CreateMap<ApiModel.GeoRequest, GrpcRequestModel.GeoCreateRequest>();
            CreateMap<ApiModel.GeoRequest, GrpcRequestModel.GeoUpdateRequest>();
        }
    }
}