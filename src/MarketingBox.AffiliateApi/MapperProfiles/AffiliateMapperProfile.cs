using AutoMapper;
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
            CreateMap<AffiliateBank, BankRequest>();
            CreateMap<AffiliateCompany, CompanyRequest>();
            CreateMap<AffiliateGeneralInfo, GeneralInfoRequest>();

            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateModel>()
                .ForMember(x => x.GeneralInfo, x => x.MapFrom(z => z));
            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Bank, AffiliateBank>();
            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Company, AffiliateCompany>();
            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateGeneralInfo>();
        }
    }
}