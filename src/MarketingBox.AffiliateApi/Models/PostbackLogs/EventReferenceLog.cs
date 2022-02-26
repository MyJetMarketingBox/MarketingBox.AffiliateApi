using MarketingBox.AffiliateApi.Enums;
using System;

namespace MarketingBox.AffiliateApi.Models.PostbackLogs
{
    public class EventReferenceLog
    {
        public long Id { get; set; }
        public long AffiliateId { get; set; }
        public string RegistrationUId { get; set; }
        public EventType EventType { get; set; }
        public HttpQueryType HttpQueryType { get; set; }
        public string PostbackReference { get; set; }
        public string PostbackResponse { get; set; }
        public string EventMessage { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public DateTime Date { get; set; }
        public string AffiliateName { get; set; }
    }
}
