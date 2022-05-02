using System;
using MarketingBox.Affiliate.Service.Domain.Models.Affiliates;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Affiliates.Requests
{
    public class AffiliateSearchRequest : PaginationRequest<long?>
    {
        public long? Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Phone { get; set; }
        public State? State { get; set; }
    }
}