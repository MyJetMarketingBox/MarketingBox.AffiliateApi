using AutoMapper;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.Redistribution.Service.Domain.Models;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class RegistrationsFileProfile : Profile
    {
        public RegistrationsFileProfile()
        {
            CreateMap<RegistrationsFile, RegistrationsFileHttp>();
        }
    }
}