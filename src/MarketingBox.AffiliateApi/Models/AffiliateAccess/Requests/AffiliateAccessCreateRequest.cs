using MarketingBox.AffiliateApi.Models.Campaigns;

namespace MarketingBox.AffiliateApi.Models.AffiliateAccess.Requests
{
    public class AffiliateAccessCreateRequest
    {
        public long MasterAffiliateId { get; set; }
        public long AffiliateId { get; set; }
    }
}
