using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Integrations.Requests
{
    public class IntegrationUpsertRequest
    {
        [Required]
        public string Name { get; set; }
    }
}