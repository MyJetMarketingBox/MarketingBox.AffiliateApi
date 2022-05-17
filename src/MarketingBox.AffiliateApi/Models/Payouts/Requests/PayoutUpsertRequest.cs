using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class PayoutUpsertRequest
    {
        [Required] public decimal? Amount { get; set; }

        [DefaultValue(MarketingBox.Sdk.Common.Enums.Currency.USD)]
        public Currency? Currency { get; set; }

        [Required] public Plan? PayoutType { get; set; }
        [Required] public int? GeoId { get; set; }
        [Required] public string Name { get; set; }
    }
}