namespace MarketingBox.AffiliateApi.Models.Postback
{
    public class Reference : ReferenceBase
    {
        public long Id { get; set; }
        public long AffiliateId { get; init; }
        public string AffiliateName { get; init; }
    }
}
