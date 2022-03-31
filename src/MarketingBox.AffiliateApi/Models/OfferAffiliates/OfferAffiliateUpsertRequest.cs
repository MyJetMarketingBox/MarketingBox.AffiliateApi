using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.OfferAffiliates
{
    public class OfferAffiliateUpsertRequest
    {
        [Required]
        public long? CampaignId { get; set; }
        [Required]
        public long? AffiliateId { get; set; }
        [Required]
        public long? OfferId { get; set; }
    }
}