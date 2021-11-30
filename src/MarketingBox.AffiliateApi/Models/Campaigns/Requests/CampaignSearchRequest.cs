using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Campaigns.Requests
{
    public class CampaignSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }

    }
}
