using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public List<long> BrandPayoutIds { get; set; }
        public string Link { get; set; }
        public LinkParametersModel LinkParameters { get; set; }
    }
}