using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.CampaignRows;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.CampaignRows.Requests
{
    public class CampaignRowUpsertRequest : ValidatableEntity
    {
        [Required]
        public long? BrandId { get; set; }
        [Required]
        public long? CampaignId { get; set; }
        [Required]
        public int? Priority { get; set; }
        [Required]
        public int? Weight { get; set; }
        [Required]
        public CapType? CapType { get; set; }
        [Required]
        public long? DailyCapValue { get; set; }
        [DefaultValue("All days, all hours")]
        public ActivityHours[] ActivityHours { get; set; }
        public string Information { get; set; }
        [DefaultValue(false)]
        public bool? EnableTraffic { get; set; }
        [Required]
        public int? GeoId { get; set; }
    }
}
