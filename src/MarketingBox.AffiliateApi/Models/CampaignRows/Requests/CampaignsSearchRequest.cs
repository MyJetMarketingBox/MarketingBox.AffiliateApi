using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.CampaignRows.Requests
{
    public class CampaignRowSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "campaignId")]
        public long? CampaignId { get; set; }

        [FromQuery(Name = "brandId")]
        public long? BrandId { get; set; }
    }
}
