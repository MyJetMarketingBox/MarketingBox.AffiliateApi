using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateUpdateRequestHttp : AffiliateBaseRequest
    {
        [Required]
        public AffiliateGeneralInfoBase GeneralInfo { get; set; }
    }
}