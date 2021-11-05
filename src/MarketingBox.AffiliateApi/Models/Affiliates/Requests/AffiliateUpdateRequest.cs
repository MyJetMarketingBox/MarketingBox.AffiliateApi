namespace MarketingBox.AffiliateApi.Models.Partners.Requests
{
    public class AffiliateUpdateRequest
    {
        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        public long Sequence { get; set; }
    }
}