using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.BrandBox.Requests
{
    public class BrandBoxSearchRequest : PaginationRequest<long?>
    {
        public string Name { get; set; }
    }
}