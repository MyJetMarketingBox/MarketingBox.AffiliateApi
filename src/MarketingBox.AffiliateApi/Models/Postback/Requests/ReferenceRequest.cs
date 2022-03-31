using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Postback.Requests
{
    public class ReferenceRequest : ReferenceBase
    {
        [Required]
        internal long? AffiliateId { get; set; }
    }
}
