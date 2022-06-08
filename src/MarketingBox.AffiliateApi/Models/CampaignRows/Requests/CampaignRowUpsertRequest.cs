using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.CampaignRows;
using MarketingBox.Affiliate.Service.Grpc.Attributes;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.CampaignRows.Requests
{
    public class CampaignRowUpsertRequest : ValidatableEntity
    {
        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public long? CampaignId { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public long? BrandId { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public int? GeoId { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public int? Priority { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public int? Weight { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public long? DailyCapValue { get; set; }

        [Required, IsEnum] public CapType? CapType { get; set; }

        [DefaultValue("All days, all hours"), ActivityHoursValidator]
        public List<ActivityHours> ActivityHours { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Information { get; set; }

        [DefaultValue(false)] 
        public bool? EnableTraffic { get; set; }
    }
}