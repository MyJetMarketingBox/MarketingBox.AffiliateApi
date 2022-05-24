using System;

namespace MarketingBox.AffiliateApi.Models.Campaigns
{
    public class CampaignModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActiveAt { get; set; }
    }
}
