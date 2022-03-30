using System;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.AffiliateApi.Models.Country;

namespace MarketingBox.AffiliateApi.Models.Payouts
{
    public class AffiliatePayoutModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public PayoutType PayoutType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public GeoModel Geo { get; set; }
    }
}