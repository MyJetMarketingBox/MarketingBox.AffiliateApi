using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Brands.Requests
{
    public class BrandsSearchRequest : PaginationRequest<long?>
    {
        [FromQuery]
        public long? Id { get; set; }

        [FromQuery]
        public string Name { get; set; }

        [FromQuery]
        public long? IntegrationId { get; set; }
        
        [FromQuery]
        public IntegrationType? IntegrationType { get; set; }
    }
}
