using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Offers;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferUpsertRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<int> GeoIds { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        [Required]
        public int? LanguageId { get; set; }
        [Required]
        public long? BrandId { get; set; }
        public OfferPrivacy? Privacy { get; set; }
        public OfferState? State { get; set; }
    }
}