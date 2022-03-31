using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateUpsertRequest
    {
        [Required]
        public AffiliateGeneralInfo GeneralInfo { get; set; }

        public AffiliateCompany Company { get; set; }

        public AffiliateBank Bank { get; set; }
        public List<long> AffiliatePayoutIds { get; set; }
    }
}