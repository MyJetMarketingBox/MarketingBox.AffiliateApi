using MarketingBox.Reporting.Service.Domain.Models;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "affiliateId")]
        public long? AffiliateId { get; set; }
        
        [FromQuery(Name = "reportType")]
        public RegistrationsReportType? Type { get; set; }
    }
}
