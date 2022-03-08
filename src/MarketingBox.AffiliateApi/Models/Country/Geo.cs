using System;

namespace MarketingBox.AffiliateApi.Models.Country
{
    public class Geo
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public int[] CountryIds { get; set; }
    }
}