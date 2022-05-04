using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.AffiliateApi.Models.Registrations.Requests;
using MarketingBox.Redistribution.Service.Domain.Models;

namespace MarketingBox.AffiliateApi.Models.Redistribution
{
    public class CreateRedistributionRequestHttp
    {
        [Required]
        public long? AffiliateId { get; set; }
        [Required]
        public long? CampaignId { get; set; }
        [Required]
        public RedistributionFrequency? Frequency { get; set; }
        [Required]
        public int? PortionLimit { get; set; }
        [Required]
        public int? DayLimit { get; set; }
        [Required]
        public bool? UseAutologin { get; set; }
        public List<long> RegistrationsIds { get; set; }
        public List<long> FilesIds { get; set; }
        public RegistrationSearchRequest RegistrationSearchRequest { get; set; }
    }
}