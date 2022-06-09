using AutoMapper;
using MarketingBox.Postback.Service.Domain.Models;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class PostbackLogsProfile:Profile
    {
        public PostbackLogsProfile()
        {
            CreateMap<EventReferenceLog, Models.PostbackLogs.EventReferenceLog>()
                .ForMember(
                    d => d.AffiliateName,
                    m =>
                        m.MapFrom(s => s.Affiliate.Name))
                .ForMember(
                    d => d.ResponseStatus,
                    m => m.MapFrom(x => x.PostbackResponseStatus));
        }
    }
}
