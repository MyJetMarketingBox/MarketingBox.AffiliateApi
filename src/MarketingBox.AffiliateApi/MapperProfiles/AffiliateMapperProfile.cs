using AutoMapper;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Grpc.Requests.Affiliates;
using MarketingBox.AffiliateApi.Models.Affiliates;
using MarketingBox.AffiliateApi.Models.Affiliates.Requests;
using AffiliateCreateRequest = MarketingBox.Affiliate.Service.Grpc.Requests.Affiliates.AffiliateCreateRequest;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class AffiliateMapperProfile : Profile
    {
        public AffiliateMapperProfile()
        {
            CreateMap<AffiliateCreateRequestHttp, AffiliateCreateRequest>();
            CreateMap<AffiliateUpdateRequestHttp, AffiliateUpdateRequest>();
            CreateMap<AffiliateBank, Bank>();
            CreateMap<AffiliateCompany, Company>();
            CreateMap<AffiliateGeneralInfoBase, GeneralInfoRequest>();
            CreateMap<AffiliateGeneralInfoCreate, GeneralInfoRequest>();

            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateModel>()
                .ForMember(x => x.GeneralInfo, x => x.MapFrom(z => z));
            CreateMap<Bank, AffiliateBank>();
            CreateMap<Company, AffiliateCompany>();
            CreateMap<Affiliate.Service.Domain.Models.Affiliates.Affiliate, AffiliateGeneralInfoBase>();
        }
    }
}