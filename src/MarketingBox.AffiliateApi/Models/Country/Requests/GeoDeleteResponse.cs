using System.Collections.Generic;
using MarketingBox.Affiliate.Service.Domain.Models.Country;

namespace MarketingBox.AffiliateApi.Models.Country.Requests;

public class GeoDeleteResponse
{
    public bool IsOk { get; set; }
    public IReadOnlyCollection<GeoRemoveResponse> Items { get; set; }
}