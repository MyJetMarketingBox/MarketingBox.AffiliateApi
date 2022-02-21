using AutoMapper;
using MarketingBox.Postback.Service.Domain.Models;

namespace MarketingBox.AffiliateApi.MapperProfiles
{
    public class PostbackLogsProfile:Profile
    {
        public PostbackLogsProfile()
        {
            CreateMap<EventReferenceLog, Models.PostbackLogs.EventReferenceLog>();
        }
    }
}
