using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Country.Requests
{
    public class GeoUpsertRequest
    {
        [Required]
        public string Name{ get; set; }
        [Required]
        public int[] CountryIds { get; set; }
    }
}