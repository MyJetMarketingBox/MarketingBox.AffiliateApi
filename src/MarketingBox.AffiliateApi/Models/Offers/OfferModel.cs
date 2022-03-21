using System.Collections.Generic;

namespace MarketingBox.AffiliateApi.Models.Offers
{
    public class OfferModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public ICollection<OfferSubParameterModel> Parameters { get; set; }
    }
}