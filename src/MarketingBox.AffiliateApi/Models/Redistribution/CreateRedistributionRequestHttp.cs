using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;
using MarketingBox.Redistribution.Service.Domain.Models;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Models;

namespace MarketingBox.AffiliateApi.Models.Redistribution
{
    public class CreateRedistributionRequestHttp : ValidatableEntity
    {
        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public long? AffiliateId { get; set; }

        [Required, AdvancedCompare(ComparisonType.GreaterThan, 0)]
        public long? CampaignId { get; set; }

        [Required, IsEnum] public RedistributionFrequency? Frequency { get; set; }
        [Required] public int? PortionLimit { get; set; }
        [Required] public int? DayLimit { get; set; }
        [Required] public bool? UseAutologin { get; set; }
        public List<long> RegistrationsIds { get; set; }
        public List<long> FilesIds { get; set; }
        public RegistrationSearchRequest RegistrationSearchRequest { get; set; }

        [Required, StringLength(128, MinimumLength = 1)]
        public string Name { get; set; }
    }
}