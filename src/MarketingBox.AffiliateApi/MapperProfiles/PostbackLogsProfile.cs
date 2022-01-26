using AutoMapper;
using MarketingBox.Postback.Service.Grpc.Models;

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
