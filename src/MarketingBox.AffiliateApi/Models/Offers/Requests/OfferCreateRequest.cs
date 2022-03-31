using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferCreateRequest
    {
        [Required]
        public long? BrandId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        public List<OfferSubParameterRequest> Parameters { get; set; }
    }
}