using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Integrations.Requests
{
    public class IntegrationsSearchRequest : PaginationRequest<long?>
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}
