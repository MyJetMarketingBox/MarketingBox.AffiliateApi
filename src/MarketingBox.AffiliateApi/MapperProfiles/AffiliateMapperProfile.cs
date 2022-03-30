using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Grpc.Requests.Affiliates;
using MarketingBox.AffiliateApi.Models.Affiliates;
using MarketingBox.AffiliateApi.Models.Affiliates.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class AffiliateMapperProfile : Profile
    {
        public AffiliateMapperProfile()
        {
            CreateMap<AffiliateUpsertRequest, AffiliateCreateRequest>();
            CreateMap<AffiliateUpsertRequest, AffiliateUpdateRequest>();
            CreateMap<AffiliateBank, Bank>();
            CreateMap<AffiliateCompany, Company>();
            CreateMap<AffiliateGeneralInfo, GeneralInfoRequest>();

            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateModel>()
                .ForMember(x => x.GeneralInfo, x => x.MapFrom(z => z));
            CreateMap<Bank, AffiliateBank>();
            CreateMap<Company, AffiliateCompany>();
            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateGeneralInfo>();
        }
    }
}