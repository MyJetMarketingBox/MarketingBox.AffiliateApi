using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Redistribution
{
    public class GetRedistributionsRequestHttp : PaginationRequest<long?>
    {
        public long? AffiliateId { get; set; }
        public long? CampaignId { get; set; }
        public long? CreatedBy { get; set; }
        public string Name { get; set; }
    }
}