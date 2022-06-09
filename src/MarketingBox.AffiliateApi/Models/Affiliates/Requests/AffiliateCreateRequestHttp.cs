using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateCreateRequestHttp : AffiliateBaseRequest
    {
        [Required]
        public AffiliateGeneralInfoCreate GeneralInfo { get; set; }
    }
}