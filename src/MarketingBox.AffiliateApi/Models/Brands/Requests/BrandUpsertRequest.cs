using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Affiliate.Service.Domain.Models.Integrations;

namespace MarketingBox.AffiliateApi.Models.Brands.Requests
{
    public class BrandUpsertRequest
    {
        public string Name { get; set; }

        public long? IntegrationId { get; set; }
        
        public IntegrationType? IntegrationType { get; set; }

        public ICollection<long> BrandPayoutIds { get; set; }
        
        public BrandStatus? Status { get; set; }

        public BrandPrivacy? Privacy { get; set; }
    }
}