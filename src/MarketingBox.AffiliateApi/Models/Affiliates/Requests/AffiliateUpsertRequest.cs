namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateUpsertRequest
    {
        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
    }
}