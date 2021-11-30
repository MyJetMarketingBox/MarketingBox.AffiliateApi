using System;
using MarketingBox.AffiliateApi.Models.Partners;
using MarketingBox.AffiliateApi.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateSearchRequest : PaginationRequest<long?>
    {
        [FromQuery(Name = "id")]
        public long? Id { get; set; }

        [FromQuery(Name = "username")]
        public string Username { get; set; }

        [FromQuery(Name = "role")]
        public AffiliateRole? Role { get; set; }

        [FromQuery(Name = "email")]
        public string Email { get; set; }

        [FromQuery(Name = "createdAt")]
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
