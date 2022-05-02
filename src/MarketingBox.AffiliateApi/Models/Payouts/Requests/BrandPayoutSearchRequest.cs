using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Common;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Payouts.Requests
{
    public class BrandPayoutSearchRequest : PaginationRequest<long?>
    {
        public long? BrandId { get; set; }
        public string Name { get; set; }
        public List<long> GeoIds { get; set; }
        public List<PayoutType> PayoutTypes { get; set; }
    }
}