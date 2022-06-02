using System;
using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Affiliate.Service.Domain.Models.OfferAffiliates;
using MarketingBox.AffiliateApi.Models.OfferAffiliates;
using MarketingBox.AffiliateApi.Models.Payouts;

namespace MarketingBox.AffiliateApi.Models.Affiliates
{
    public class AffiliateModel
    {
        public long Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public AffiliateGeneralInfoBase GeneralInfoBase { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        
        public List<AffiliatePayoutModel> Payouts { get; set; } = new ();

        public List<OfferAffiliateModel> OfferAffiliates { get; set; } = new ();
    }
}
