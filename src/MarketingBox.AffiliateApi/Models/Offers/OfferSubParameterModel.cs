namespace MarketingBox.AffiliateApi.Models.Offers
{
    public class OfferSubParameterModel
    {
        public long Id { get; set; }
        public long OfferId { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
    }
}