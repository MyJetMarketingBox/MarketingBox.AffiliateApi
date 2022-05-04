using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Enums;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationUpdateStatusRequest
    {
        [Required] public RegistrationStatus Status { get; set; }
        [Required] public string Comment { get; set; }
    }
}