using System.ComponentModel.DataAnnotations;
using MarketingBox.AffiliateApi.Enums;

namespace MarketingBox.AffiliateApi.Models.Postback
{
    public class ReferenceBase
    {
        public string RegistrationReference { get; init; }

        public string RegistrationTGReference { get; init; }

        public string DepositReference { get; init; }

        public string DepositTGReference { get; init; }
        [Required]
        public HttpQueryType? HttpQueryType { get; init; }
    }
}