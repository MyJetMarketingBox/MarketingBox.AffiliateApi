using System.ComponentModel.DataAnnotations;
using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        [Required]
        public long? AffiliateId { get; set; }
        
        [Required]
        public RegistrationsReportType? Type { get; set; }
    }
}
