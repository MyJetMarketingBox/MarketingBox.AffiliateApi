using System.Collections.Generic;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Country.Requests
{
    public class CountrySearchRequest : PaginationRequest<int?>
    {
        public string Name { get; set; }
        public string CountryIds { get; set; }
        public long? GeoId { get; set; }
    }
}