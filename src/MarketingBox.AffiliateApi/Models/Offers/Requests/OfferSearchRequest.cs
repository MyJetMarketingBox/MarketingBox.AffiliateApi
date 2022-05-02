using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferSearchRequest : PaginationRequest<long?>
    {
        public string OfferName { get; set; }
        public List<int> LanguageIds { get; set; }
        public List<OfferPrivacy> Privacies { get; set; }
        public List<OfferState> States { get; set; }
        public List<long> BrandIds { get; set; }
        public List<int> GeoIds { get; set; }
        public long? OfferId { get; set; }
    }
}