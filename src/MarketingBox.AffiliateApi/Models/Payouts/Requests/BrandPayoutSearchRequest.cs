using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class BrandPayoutSearchRequest : PaginationRequest<long?>
    {
        public long? BrandId { get; set; }
    }
}