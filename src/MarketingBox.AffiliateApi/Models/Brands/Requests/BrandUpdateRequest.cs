using MarketingBox.AffiliateApi.Models.Partners;

namespace MarketingBox.AffiliateApi.Models.Campaigns.Requests
{
    public class BrandUpdateRequest
    {
        public string Name { get; set; }

        public long IntegrationId { get; set; }

        public Payout Payout { get; set; }

        public Revenue Revenue { get; set; }

        public BrandStatus Status { get; set; }

        public BrandPrivacy Privacy { get; set; }
        public long Sequence { get; set; }
    }
}