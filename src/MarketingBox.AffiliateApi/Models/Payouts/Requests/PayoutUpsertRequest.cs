using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Common;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class PayoutUpsertRequest
    {
        [Required] public decimal? Amount { get; set; }

        [DefaultValue(Affiliate.Service.Domain.Models.Common.Currency.USD)]
        public Currency? Currency { get; set; }

        [Required] public PayoutType? PayoutType { get; set; }
        [Required] public int? GeoId { get; set; }
        [Required] public string Name { get; set; }
    }
}