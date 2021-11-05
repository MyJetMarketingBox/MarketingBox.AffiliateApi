using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Campaigns.Requests
{
    public class BrandsSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "integrationId")]
        public long? IntegrationId { get; set; }

        [FromQuery(Name = "status")]
        public BrandStatus? Status { get; set; }
    }
}
