namespace MarketingBox.AffiliateApi.Models.Country.Requests
{
    public class GeoUpsertRequest
    {
        public string Name{ get; set; }
        public int[] CountryIds { get; set; }
    }
}