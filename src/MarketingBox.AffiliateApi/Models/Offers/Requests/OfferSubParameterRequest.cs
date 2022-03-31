using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Offers.Requests
{
    public class OfferSubParameterRequest
    {
        [Required]
        public string ParamName { get; set; }
        [Required]
        public string ParamValue { get; set; }
    }
}