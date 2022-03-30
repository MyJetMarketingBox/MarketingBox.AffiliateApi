using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Domain.Models.OfferAffiliates;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateModel
    {
        public long Id { get; set; }

        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        
        public List<AffiliatePayout> Payouts { get; set; } = new ();

        public List<OfferAffiliate> OfferAffiliates { get; set; } = new ();
    }
}
