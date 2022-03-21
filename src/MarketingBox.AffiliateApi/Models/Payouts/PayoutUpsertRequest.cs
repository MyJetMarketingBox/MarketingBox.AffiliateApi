using MarketingBox.Affiliate.Service.Domain.Models.Common;

namespace MarketingBox.AffiliateApi.Models.Payouts
{
    public class PayoutUpsertRequest
    {
        public decimal? Amount { get; set; }

        public Currency Currency { get; set; }

        public PayoutType? PayoutType { get; set; }

        public int? GeoId { get; set; }
    }
}