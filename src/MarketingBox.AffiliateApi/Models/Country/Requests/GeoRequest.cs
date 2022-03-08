namespace MarketingBox.AffiliateApi.Models.Country.Requests
{
    public class GeoRequest
    {
        public string Name{ get; set; }
        public int[] CountryIds { get; set; }
    }
}