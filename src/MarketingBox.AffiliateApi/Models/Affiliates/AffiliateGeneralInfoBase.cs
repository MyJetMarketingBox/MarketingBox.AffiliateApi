using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateGeneralInfoBase
    {
        [Required, StringLength(128, MinimumLength = 1)]
        public string Username { get; set; }

        [Required, IsValidEmail]
        public string Email { get; set; }

        [Phone, StringLength(20, MinimumLength = 7)]
        public string Phone { get; set; }

        [StringLength(128, MinimumLength = 1)]
        public string Skype { get; set; }
        
        [StringLength(128, MinimumLength = 1)]
        public string ZipCode { get; set; }

        [IsEnum,DefaultValue(MarketingBox.Affiliate.Service.Domain.Models.Affiliates.State.Active)]
        public State? State { get; set; }

        [IsEnum,DefaultValue(Sdk.Common.Enums.Currency.USD)]
        public Currency? Currency { get; set; }
    }
}