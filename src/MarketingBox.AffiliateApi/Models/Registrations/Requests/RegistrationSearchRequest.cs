using System;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        public long? AffiliateId { get; set; }
        [Required]
        public RegistrationsReportType? Type { get; set; }
        public string? Country { get; set; }
        public RegistrationStatus? Status { get; set; }
        public CrmStatus? CrmStatus { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public long? RegistrationId { get; set; }
    }
}
