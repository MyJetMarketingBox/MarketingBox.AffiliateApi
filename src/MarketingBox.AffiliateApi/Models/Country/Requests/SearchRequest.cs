using System.Collections.Generic;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Country.Requests
{
    public class SearchRequest : PaginationRequest<int?>
    {
        public string Name { get; set; }
        public List<int> CountryIds { get; set; }
        public long? GeoId { get; set; }
    }
}