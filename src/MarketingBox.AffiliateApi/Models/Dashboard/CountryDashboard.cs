namespace MarketingBox.AffiliateApi.Models.Dashboard;

public class CountryDashboard
{
    public int CountryId { get; set; }
    public string Alpha2Code { get; set; }
    public int RegistrationsCount { get; set; }
    public int FtdCount { get; set; }
    public int FailedCount { get; set; }
    public double Cr { get; set; }
}