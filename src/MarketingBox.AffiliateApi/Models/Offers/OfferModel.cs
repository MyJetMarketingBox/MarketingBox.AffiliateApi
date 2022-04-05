using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;

namespace MarketingBox.AffiliateApi.Models.Offers
{
    public class OfferModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public List<int> GeoIds { get; set; }
        public Currency? Currency { get; set; }
        public int? LanguageId { get; set; }
        public OfferPrivacy? Privacy { get; set; }
        public OfferState? State { get; set; }
        public long? BrandId { get; set; }
    }
}