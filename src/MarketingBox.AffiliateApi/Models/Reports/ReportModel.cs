namespace MarketingBox.AffiliateApi.Models.Reports
{
    public class ReportModel
    {
        public long AffiliateId { get; set; }

        public long RegistrationsCount { get; set; }

        public long FtdCount { get; set; }

        public decimal Payout { get; set; }

        public decimal Revenue { get; set; }

        public decimal Cr { get; set; }
    }
}