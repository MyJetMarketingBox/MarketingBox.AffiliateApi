using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Attributes;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateGeneralInfoCreate : AffiliateGeneralInfoBase
    {    
        [IsValidPassword, StringLength(128, MinimumLength = 1)]
        public string Password { get; set; }
    }
}