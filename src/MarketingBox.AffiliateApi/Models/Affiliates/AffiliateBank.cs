using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateBank : ValidatableEntity
    {
        [StringLength(128, MinimumLength = 1)] public string BeneficiaryName { get; set; }

        [StringLength(128, MinimumLength = 1)] public string BeneficiaryAddress { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Name { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Address { get; set; }

        [StringLength(128, MinimumLength = 1)] public string AccountNumber { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Swift { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Iban { get; set; }
    }
}