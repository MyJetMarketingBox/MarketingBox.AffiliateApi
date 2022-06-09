using System;
using System.Collections.Generic;

namespace MarketingBox.AffiliateApi.Models.BrandBox
{
    public class BrandBoxModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<long> BrandIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
    }
}