namespace MarketingBox.AffiliateApi.Models.Reports
{
    public class ReportModel
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public long RegistrationCount { get; set; }

        public long FtdCount { get; set; }

        public long FailedCount { get; set; }

        public long UnassignedCount { get; set; }

        public decimal Payout { get; set; }

        public decimal Revenue { get; set; }

        public decimal? Cr { get; set; }

        public decimal Pl { get; set; }

        public decimal? Epc { get; set; }

        public decimal? Epl { get; set; }

        public decimal? Roi { get; set; }

        public decimal? Clicks { get; set; }
    }
}