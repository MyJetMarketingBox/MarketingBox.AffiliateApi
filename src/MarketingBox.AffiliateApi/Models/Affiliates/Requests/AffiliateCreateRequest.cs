using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Partners.Requests
{
    public class AffiliateCreateRequest
    {
        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
    }
}
