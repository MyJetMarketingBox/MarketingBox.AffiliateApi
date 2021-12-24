using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "affiliateId")]
        public long? AffiliateId { get; set; }
    }
}
