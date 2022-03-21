using MarketingBox.AffiliateApi.Models.Affiliates;
using MarketingBox.AffiliateApi.Models.Campaigns;
using MarketingBox.AffiliateApi.Models.Offers;

namespace MarketingBox.AffiliateApi.Models.OfferAffiliates
{
    public class OfferAffiliateModel
    {
        public long Id { get; set; }
        public CampaignModel Campaign { get; set; }
        public AffiliateModel Affiliate { get; set; }
        public OfferModel Offer { get; set; }
    }
}