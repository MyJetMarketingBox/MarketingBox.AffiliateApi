using System;
using MarketingBox.AffiliateApi.Models.Country;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Payouts
{
    public class BrandPayoutModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
        public Plan PayoutType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public GeoModel Geo { get; set; }
    }
}