using System;

namespace MarketingBox.AffiliateApi.Models.Reports
{
    public class ReportByDaysModel
    {
        public DateTimeOffset CreatedAt { get; set; }

        public long LeadCount { get; set; }

        public long FtdCount { get; set; }
    }
}