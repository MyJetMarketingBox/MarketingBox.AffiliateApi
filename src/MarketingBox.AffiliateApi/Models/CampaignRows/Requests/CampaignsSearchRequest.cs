using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.CampaignRows;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.CampaignRows.Requests
{
    public class CampaignRowSearchRequest : PaginationRequest<long?>
    {
        public long? Id { get; set; }
        public List<long> CampaignIds { get; set; }
        public long? BrandId { get; set; }
        public int? Priority { get; set; }
        public int? Weight { get; set; }
        public CapType? CapType { get; set; }
        public bool? EnableTraffic { get; set; }
        public List<long> GeoIds { get; set; }
        public long? DailyCapValue { get; set; }
    }
}
