namespace MarketingBox.AffiliateApi.Models.Registrations
{
    public class RegistrationRouteInfo
    {
        public long AffiliateId { get; set; }
        public string AffiliateName { get; set; }

        public long CampaignId { get; set; }

        public long BrandId { get; set; }
        public string BrandName { get; set; }

        public long IntegrationIdId { get; set; }
        public string IntegrationName { get; set; }
    }
}