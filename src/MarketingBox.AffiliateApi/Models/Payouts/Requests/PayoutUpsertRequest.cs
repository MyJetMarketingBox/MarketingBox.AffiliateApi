using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class PayoutUpsertRequest : ValidatableEntity
    {
        [Required, AdvancedCompare(ComparisonType.GreaterThanOrEqual, 0)]
        public decimal? Amount { get; set; }

        [DefaultValue(MarketingBox.Sdk.Common.Enums.Currency.USD), IsEnum]
        public Currency? Currency { get; set; }

        [Required, IsEnum] public Plan? PayoutType { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public int? GeoId { get; set; }

        [Required, StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
    }
}