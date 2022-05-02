using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.OfferAffiliates
{
    public class SearchRequest : PaginationRequest<long?>
    {
        public long? OfferId { get; set; }
    }
}