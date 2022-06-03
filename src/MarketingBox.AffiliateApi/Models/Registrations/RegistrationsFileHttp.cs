using System;

namespace MarketingBox.AffiliateApi.Models.Registrations
{
    public class RegistrationsFileHttp
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public long CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}