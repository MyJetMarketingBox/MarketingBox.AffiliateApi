namespace MarketingBox.AffiliateApi.Models.OfferAffiliates
{
    public class OfferAffiliateModel
    {
        public long Id { get; set; }
        public long CampaignId { get; set; }
        public long AffiliateId { get; set; }
        public long OfferId { get; set; }
    }
}