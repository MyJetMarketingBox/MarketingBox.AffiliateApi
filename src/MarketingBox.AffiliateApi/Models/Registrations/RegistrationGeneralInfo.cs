using System;
using Destructurama.Attributed;

namespace MarketingBox.AffiliateApi.Models.Leads
{
    public class RegistrationGeneralInfo
    {
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string FirstName { get; set; }

        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string LastName { get; set; }

        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Email { get; set; }

        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Phone { get; set; }
        
        [LogMasked(PreserveLength = true, ShowFirst = 2, ShowLast = 2)]
        public string Ip { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DepositedAt { get; set; }
        public DateTime? ConversionDate { get; set; }
        public string Country { get; set; }
    }

    public class RegistrationGeneralInfoForAffiliate
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? DepositedAt { get; set; }
        public DateTime? ConversionDate { get; set; }
        public string Country { get; set; }
    }
}