using MarketingBox.AffiliateApi.Models.Campaigns;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.AffiliateAccess.Requests
{
    public class AffiliateAccessSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "masteraffiliateid")]
        public long? MasterAffiliateId { get; set; }

        [FromQuery(Name = "affiliateid")]
        public long? AffiliateId { get; set; }
    }
}
