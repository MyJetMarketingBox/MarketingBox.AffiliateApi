using AutoMapper;
using MarketingBox.Affiliate.Service.Grpc.Requests;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class CommonRequestsMapperProfile : Profile
    {
        public CommonRequestsMapperProfile()
        {
            CreateMap<Models.SearchByNameRequest, SearchByNameRequest>()
                .ForMember(x => x.Asc, x => x.MapFrom(z => z.Order == PaginationOrder.Asc))
                .ForMember(x => x.Take, x => x.MapFrom(z => z.Limit));
        }
    }
}