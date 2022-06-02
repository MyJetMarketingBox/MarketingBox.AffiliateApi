using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateCompany : ValidatableEntity
    {
        [StringLength(128, MinimumLength = 1)] public string Name { get; set; }

        [StringLength(128, MinimumLength = 1)] public string Address { get; set; }

        [StringLength(128, MinimumLength = 1)] public string RegNumber { get; set; }

        [StringLength(128, MinimumLength = 1)] public string VatId { get; set; }
    }
}