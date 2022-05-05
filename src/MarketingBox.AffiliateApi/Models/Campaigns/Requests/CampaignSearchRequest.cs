using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Campaigns.Requests
{
    public class CampaignSearchRequest : PaginationRequest<long?>
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}
