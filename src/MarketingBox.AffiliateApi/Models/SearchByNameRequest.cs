using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models
{
    public class SearchByNameRequest : PaginationRequest<long?>
    {
        public string Name { get; set; }
    }
}