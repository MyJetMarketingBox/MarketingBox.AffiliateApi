using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Domain.Models.Integrations;
using MarketingBox.AffiliateApi.Models.CampaignRows;
using MarketingBox.AffiliateApi.Models.Integrations;
using MarketingBox.AffiliateApi.Models.Payouts;

namespace MarketingBox.AffiliateApi.Models.Brands
{
    public class BrandModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IntegrationModel Integration { get; set; }

        public IntegrationType IntegrationType { get; set; }

        public List<CampaignRowModel> CampaignRows { get; set; } = new ();

        public List<BrandPayoutModel> Payouts { get; set; } = new ();
        public string Link { get; set; }
        public LinkParametersModel LinkParameters { get; set; }
    }
}