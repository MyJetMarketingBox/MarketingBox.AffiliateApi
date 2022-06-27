using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.CampaignRows.Requests;

public class UpdateTrafficRequestHttp : ValidatableEntity
{
    [Required]
    public bool? EnableTraffic { get; set; }
}