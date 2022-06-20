using System.Collections.Generic;

namespace MarketingBox.AffiliateApi.Models.Dashboard;

public class DashboardMap
{
    public List<CountryDashboard> Countries { get; set; } = new();
}