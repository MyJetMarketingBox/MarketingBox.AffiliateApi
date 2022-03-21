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

        public IntegrationModel Integration { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public BrandStatus Status { get; set; }

        public BrandPrivacy Privacy { get; set; }

        public ICollection<CampaignRowModel> CampaignRows { get; set; } = new List<CampaignRowModel>();

        public ICollection<BrandPayout> Payouts { get; set; } = new List<BrandPayout>();
    }
}