using System.ComponentModel.DataAnnotations;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.CampaignBoxes.Requests
{
    public class CampaignBoxesSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "campaignId")]
        public long? CampaignId { get; set; }

        [Required, FromQuery(Name = "brandId")]
        public long BrandId { get; set; }
    }
}
