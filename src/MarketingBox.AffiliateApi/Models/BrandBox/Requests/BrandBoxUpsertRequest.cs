using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.BrandBox.Requests
{
    public class BrandBoxUpsertRequest
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public List<long> BrandIds { get; set; }
    }
}