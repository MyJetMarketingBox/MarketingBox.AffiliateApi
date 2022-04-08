using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferSearchRequest : PaginationRequest<long?>
    {
        public string OfferName { get; set; }
        public int? LanguageId { get; set; }
        public OfferPrivacy? Privacy { get; set; }
        public OfferState? State { get; set; }
        public Currency? Currency { get; set; }
        public long? BrandId { get; set; }
    }
}