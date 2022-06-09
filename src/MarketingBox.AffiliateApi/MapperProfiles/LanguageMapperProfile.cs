using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Languages;
using MarketingBox.AffiliateApi.Models.Language;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class LanguageMapperProfile : Profile
    {
        public LanguageMapperProfile()
        {
            CreateMap<Language, LanguageModel>();
        }
    }
}