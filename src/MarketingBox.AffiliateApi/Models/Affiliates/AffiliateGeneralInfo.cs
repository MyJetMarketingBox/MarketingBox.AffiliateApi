using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Domain.Models.Common;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateGeneralInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string ZipCode { get; set; }
        public State? State { get; set; }
        public Currency? Currency { get; set; }
    }
}