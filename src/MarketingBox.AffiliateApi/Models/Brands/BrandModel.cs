using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Domain.Models.Integrations;
using MarketingBox.AffiliateApi.Models.CampaignRows;
using MarketingBox.AffiliateApi.Models.Integrations;

namespace MarketingBox.AffiliateApi.Models.Brands
{
    public class BrandModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long? IntegrationId { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public BrandStatus Status { get; set; }

        public BrandPrivacy Privacy { get; set; }

        public List<CampaignRowModel> CampaignRows { get; set; } = new ();

        public List<BrandPayout> Payouts { get; set; } = new ();
    }
}