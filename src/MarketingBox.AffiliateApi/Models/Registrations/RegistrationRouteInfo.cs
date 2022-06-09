namespace MarketingBox.AffiliateApi.Models.Registrations
{
    public class RegistrationRouteInfo
    {
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }

        public long? CampaignId { get; set; }
        public string CampaignName { get; set; }

        public long? BrandId { get; set; }
        public string BrandName { get; set; }

        public long? IntegrationId { get; set; }
        public string IntegrationName { get; set; }

        public long? OfferId { get; set; }
        public string OfferName { get; set; }
    }
}