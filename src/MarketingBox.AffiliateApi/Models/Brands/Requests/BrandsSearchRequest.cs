using MarketingBox.Affiliate.Service.Domain.Models.Brands;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Brands.Requests
{
    public class BrandsSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }

        [FromQuery(Name = "integrationId")]
        public long? IntegrationId { get; set; }
    }
}
