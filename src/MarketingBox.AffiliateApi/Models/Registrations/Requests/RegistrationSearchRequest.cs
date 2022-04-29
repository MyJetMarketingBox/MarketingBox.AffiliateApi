using System;
using System.Collections.Generic;
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
        public List<long> BrandBoxIds { get; set; }
        public RegistrationStatus? Status { get; set; }
        public CrmStatus? CrmStatus { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public long? RegistrationId { get; set; }

        public MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest GetGrpcModel(string tenantId)
        {
            return new MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest()
            {
                Asc = Order == PaginationOrder.Asc,
                Cursor = Cursor,
                Take = Limit,
                TenantId = tenantId,
                AffiliateId = AffiliateId,
                Type = Type ?? RegistrationsReportType.All,
                Country = Country,
                Status = Status,
                CrmStatus = CrmStatus,
                DateFrom = DateFrom,
                DateTo = DateTo,
                RegistrationId = RegistrationId,
                BrandBoxIds = BrandBoxIds
            };
        }
    }
}
