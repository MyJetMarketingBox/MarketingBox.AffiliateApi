using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Domain.Models.Common;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateGeneralInfo
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Phone { get; set; }
        public string Skype { get; set; }
        public string ZipCode { get; set; }

        [DefaultValue(MarketingBox.Affiliate.Service.Domain.Models.Affiliates.State.Active)]
        public State? State { get; set; }

        [DefaultValue(MarketingBox.Affiliate.Service.Domain.Models.Common.Currency.USD)]
        public Currency? Currency { get; set; }
    }
}