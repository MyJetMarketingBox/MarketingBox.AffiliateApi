using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Domain.Models.Integrations;

namespace MarketingBox.AffiliateApi.Models.Brands.Requests
{
    public class BrandUpsertRequest
    {
        [Required]
        public string Name { get; set; }

        public long? IntegrationId { get; set; }
        
        [Required]
        public IntegrationType? IntegrationType { get; set; }

        public ICollection<long> BrandPayoutIds { get; set; }
        
        [DefaultValue(BrandStatus.Active)]
        public BrandStatus? Status { get; set; }

        [DefaultValue(BrandPrivacy.Public)]
        public BrandPrivacy? Privacy { get; set; }
    }
}