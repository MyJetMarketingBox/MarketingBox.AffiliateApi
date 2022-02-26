using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Integrations.Requests
{
    public class IntegrationsSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }
    }
}
