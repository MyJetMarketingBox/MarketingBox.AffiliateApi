using AutoMapper;
using MarketingBox.AffiliateApi.Models.Registrations;
using MarketingBox.Postback.Service.Domain.Models;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Registration.Service.Domain.Models.Registrations.Deposit;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class RegistrationsFileProfile : Profile
    {
        public RegistrationsFileProfile()
        {
            CreateMap<RegistrationsFile, RegistrationsFileHttp>()
                .ForMember(x => x.CreatedByUserId, x => x.MapFrom(z => z.CreatedBy))
                .ForMember(x => x.CreatedByUserName, x => x.MapFrom("Some user"));
        }
    }
}