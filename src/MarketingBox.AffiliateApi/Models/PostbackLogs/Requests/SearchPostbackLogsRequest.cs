using System;
using MarketingBox.Postback.Service.Domain.Models;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.PostbackLogs.Requests
{
    public class SearchPostbackLogsRequest : PaginationRequest<long?>
    {
        public string AffiliateIds { get; set; }
        public string AffiliateName { get; set; }        
        public EventType? EventType { get; set; }
        public HttpQueryType? HttpQueryType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RegistrationUId { get; set; }
    }
}