using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferSearchRequest : PaginationRequest<long?>
    {
        public string OfferName { get; set; }
        public string LanguageIds { get; set; }
        public string Privacies { get; set; }
        public string States { get; set; }
        public string BrandIds { get; set; }
        public string GeoIds { get; set; }
        public long? OfferId { get; set; }
    }
}