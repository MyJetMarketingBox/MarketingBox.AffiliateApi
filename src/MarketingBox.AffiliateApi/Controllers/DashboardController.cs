using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Models.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarketingBox.AffiliateApi.Controllers;

[Route("/api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly ILogger<DashboardController> _logger;
    private Dashboard _dashboard;

    public DashboardController(ILogger<DashboardController> logger)
    {
        _logger = logger;
    }

    private static StatParameter GetStat(int from, int to)
    {
        var rnd = new Random();
        return new StatParameter
        {
            Actual = rnd.Next(from, to),
            Percent = rnd.Next(1, 10),
            DiffType = Enum.GetValues<DiffType>()[rnd.Next(0, 2)]
        };
    }

    private static Dashboard GetDashboard()
    {
        return new Dashboard
        {
            Clicks = GetStat(100000, 500000),
            Cr = GetStat(20000, 50000),
            Payouts = GetStat(40000, 50000),
            FailedCount = GetStat(10, 50),
            FtdCount = GetStat(3000, 4000),
            RegistrationsCount = GetStat(20000, 60000),
        };
    }

    private static List<CountryDashboard> GetCountries(Dashboard dashboard)
    {
        var countries = new Dictionary<int, string>
        {
            [76] = "FR",
            [14] = "AU",
            [232] = "UA",
            [236] = "US",
            [227] = "TR"
        };

        var rnd = new Random();
        return Enumerable.Range(0, rnd.Next(1,3)).Select(x =>
        {
            var countryId = countries.Keys.ToList()[rnd.Next(0, 4)];
            return new CountryDashboard
            {
                Cr = dashboard.Cr.Actual,
                FailedCount = (int) dashboard.Cr.Actual,
                FtdCount = (int) dashboard.Cr.Actual,
                RegistrationsCount = (int) dashboard.RegistrationsCount.Actual,
                CountryId = countryId,
                Alpha2Code = countries[countryId]
            };
        }).ToList();
    }

    [HttpGet]
    public async Task<ActionResult<Dashboard>> GetDashboard(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        _logger.LogInformation("Get dashboard for period from {@from} to {@to} ", fromDate, toDate);
        _dashboard = GetDashboard();
        return await Task.FromResult(_dashboard);
    }

    [HttpGet("map")]
    public async Task<ActionResult<DashboardMap>> GetDashboardMap(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        _logger.LogInformation("Get dashboard map for period from {@from} to {@to} ", fromDate, toDate);
        var dashboard = _dashboard ?? GetDashboard();
        return await Task.FromResult(new DashboardMap
        {
            Countries = GetCountries(dashboard)
        });
    }
}