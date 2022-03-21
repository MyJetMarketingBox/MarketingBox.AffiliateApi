namespace MarketingBox.AffiliateApi.Models.OfferAffiliates
{
    public class OfferAffiliateUpsertRequest
    {
        public long? CampaignId { get; set; }

        public long? AffiliateId { get; set; }
        
        public long? OfferId { get; set; }
    }
}