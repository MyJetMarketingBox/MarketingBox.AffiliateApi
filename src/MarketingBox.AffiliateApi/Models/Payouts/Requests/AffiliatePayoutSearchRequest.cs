using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class AffiliatePayoutSearchRequest : PaginationRequest<long?>
    {
        public long? AffiliateId { get; set; }
    }
}