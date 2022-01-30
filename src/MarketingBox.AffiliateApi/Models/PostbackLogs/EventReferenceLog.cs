using MarketingBox.AffiliateApi.Enums;
using System;

namespace MarketingBox.AffiliateApi.Models.PostbackLogs
{
    public class EventReferenceLog
    {
        public long AffiliateId { get; set; }
        public Status EventStatus { get; set; }
        public HttpQueryType HttpQueryType { get; set; }
        public string PostbackReference { get; set; }
        public string PostbackResult { get; set; }
        public DateTime Date { get; set; }
    }
}
