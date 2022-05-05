using MarketingBox.Postback.Service.Domain.Models;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Postback.Requests
{
    public class SearchReferenceRequest : PaginationRequest<long?>
    {
        public string AffiliateIds { get; set; }
        public string AffiliateName { get; set; }
        public HttpQueryType? HttpQueryType { get; set; }
    }
}