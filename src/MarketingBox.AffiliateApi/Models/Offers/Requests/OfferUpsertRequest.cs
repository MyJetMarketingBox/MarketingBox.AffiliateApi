using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferUpsertRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public List<int> GeoIds { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        [Required]
        public int? LanguageId { get; set; }
        public OfferPrivacy? Privacy { get; set; }
        public OfferState? State { get; set; }
        public long? BrandId { get; set; }
    }
}