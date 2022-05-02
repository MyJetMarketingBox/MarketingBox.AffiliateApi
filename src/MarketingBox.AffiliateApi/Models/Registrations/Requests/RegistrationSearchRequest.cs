using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        public List<long> AffiliateIds { get; set; }
        [Required]
        public RegistrationsReportType? Type { get; set; }
        public List<int> CountryIds { get; set; }
        public List<long> BrandBoxIds { get; set; }
        public List<RegistrationStatus> Statuses { get; set; }
        public List<CrmStatus> CrmStatuses { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public List<long> RegistrationIds { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<long> BrandIds { get; set; }
        public List<long> CampaignIds { get; set; }
        public string FirstName { get; set; }
        public List<long> IntegrationIds { get; set; }
        public string LastName { get; set; }
        
        public MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest GetGrpcModel(string tenantId)
        {
            return new MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest()
            {
                Asc = Order == PaginationOrder.Asc,
                Cursor = Cursor,
                Take = Limit,
                TenantId = tenantId,
                AffiliateIds = AffiliateIds,
                Type = Type ?? RegistrationsReportType.All,
                CountryIds = CountryIds,
                Statuses = Statuses,
                CrmStatuses = CrmStatuses,
                DateFrom = DateFrom,
                DateTo = DateTo,
                RegistrationIds = RegistrationIds,
                BrandBoxIds = BrandBoxIds,
                Email = Email,
                Phone = Phone,
                BrandIds = BrandIds,
                CampaignIds = CampaignIds,
                FirstName = FirstName,
                IntegrationIds = IntegrationIds,
                LastName = LastName
            };
        }
    }
}
