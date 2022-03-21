using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Country;
using MarketingBox.Affiliate.Service.Grpc.Requests.Country;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.AffiliateApi.Models.Country.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class CountryMapperProfile : Profile
    {
        public CountryMapperProfile()
        {
            CreateMap<Country, CountryModel>();
            CreateMap<Geo, GeoModel>();
            CreateMap<GeoUpsertRequest, GeoCreateRequest>();
            CreateMap<GeoUpsertRequest, GeoUpdateRequest>();
        }
    }
}