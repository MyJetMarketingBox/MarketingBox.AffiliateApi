using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateBaseRequest : ValidatableEntity
    {
        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        public List<long> AffiliatePayoutIds { get; set; }
    }
}