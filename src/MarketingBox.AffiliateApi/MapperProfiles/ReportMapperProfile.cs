using AutoMapper;
using MarketingBox.AffiliateApi.Models.Reports;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Domain.Models.Reports;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class ReportMapperProfile: Profile
    {
        public ReportMapperProfile()
        {
            CreateMap<Report, ReportModel>();
            CreateMap<Models.Reports.Requests.ReportSearchRequest, ReportSearchRequest>()
                .ForMember(x => x.TenantId,
                    x => x.MapFrom(z => z.TenantId))
                .ForMember(x => x.Asc,
                    x=>x.MapFrom((s,d)=>d.Asc=s.Order==PaginationOrder.Asc))
                .ForMember(x => x.Take,
                    x => x.MapFrom(z => z.Limit));
        }
    }
}