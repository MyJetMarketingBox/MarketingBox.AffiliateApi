using MarketingBox.AffiliateApi.Enums;
using System;

namespace MarketingBox.AffiliateApi.Models.PostbackLogs
{
    public class EventReferenceLog
    {
        public long AffiliateId { get; set; }
        public string RegistrationUId { get; set; }
        public EventType EventType { get; set; }
        public HttpQueryType HttpQueryType { get; set; }
        public string PostbackReference { get; set; }
        public string RequestBody { get; set; }
        public string PostbackResponse { get; set; }
        public string EventMessage { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public DateTime Date { get; set; }
    }
}
