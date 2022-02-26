using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Deposits.Requests
{
    public class DepositSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "affiliateId")]
        public long? AffiliateId { get; set; }
    }
}
