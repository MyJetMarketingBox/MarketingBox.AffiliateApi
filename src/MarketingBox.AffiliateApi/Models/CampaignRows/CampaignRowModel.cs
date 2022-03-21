
using MarketingBox.Affiliate.Service.Domain.Models.CampaignRows;

namespace MarketingBox.AffiliateApi.Models.CampaignRows
{
    public class CampaignRowModel
    {
        public long CampaignRowId { get; set; }
        public long BrandId { get; set; }
        public long CampaignId { get; set; }
        public int Priority { get; set; }
        public int Weight { get; set; }
        public CapType CapType { get; set; }
        public long DailyCapValue { get; set; }
        public ActivityHours[] ActivityHours { get; set; }
        public string Information { get; set; }
        public bool EnableTraffic { get; set; }
        public int? GeoId { get; set; }
        public string GeoName { get; set; }
    }
}