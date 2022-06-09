using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Campaigns.Requests
{
    public class CampaignUpsertRequest
    {
        [Required]
        public string Name { get; set; }
    }
}