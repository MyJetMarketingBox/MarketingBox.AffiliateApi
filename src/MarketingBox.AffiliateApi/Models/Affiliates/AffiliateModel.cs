namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateModel
    {
        public long Id { get; set; }

        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
    }
}
