using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.AffiliateApi.Models.Language;
using MarketingBox.AffiliateApi.Models.OfferAffiliates;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Offers
{
    public class OfferModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<GeoModel> Geos { get; set; }
        public List<OfferAffiliateModel> OfferAffiliates { get; set; }
        public Currency? Currency { get; set; }
        public LanguageModel Language { get; set; }
        public OfferPrivacy? Privacy { get; set; }
        public OfferState? State { get; set; }
        public long? BrandId { get; set; }
    }
}