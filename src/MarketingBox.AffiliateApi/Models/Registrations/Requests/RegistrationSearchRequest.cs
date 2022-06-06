using System;
using System.ComponentModel;
using MarketingBox.Reporting.Service.Grpc.Requests.Registrations;
using MarketingBox.Sdk.Common.Attributes;
using MarketingBox.Sdk.Common.Enums;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Models.Registrations.Requests
{
    public class RegistrationSearchRequest : PaginationRequest<long?>
    {
        public string AffiliateIds { get; set; }
        
        [IsEnum]
        public RegistrationsReportType? Type { get; set; }
        [DefaultValue(DateTimeType.RegistrationDate)]
        public DateTimeType? DateType { get; set; }
        public string CountryIds { get; set; }
        public string BrandBoxIds { get; set; }
        public string Statuses { get; set; }
        public string CrmStatuses { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string RegistrationIds { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BrandIds { get; set; }
        public string CampaignIds { get; set; }
        public string FirstName { get; set; }
        public string IntegrationIds { get; set; }
        public string LastName { get; set; }
        public string OfferIds { get; set; }
        
        public MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest GetGrpcModel(string tenantId)
        {
            return new MarketingBox.Reporting.Service.Grpc.Requests.Registrations.RegistrationSearchRequest()
            {
                Asc = Order == PaginationOrder.Asc,
                Cursor = Cursor,
                Take = Limit,
                TenantId = tenantId,
                AffiliateIds = AffiliateIds.Parse<long>(),
                Type = Type ?? RegistrationsReportType.All,
                CountryIds = CountryIds.Parse<int>(),
                Statuses = Statuses.Parse<RegistrationStatus>(),
                CrmStatuses = CrmStatuses.Parse<CrmStatus>(),
                DateFrom = DateFrom,
                DateTo = DateTo,
                RegistrationIds = RegistrationIds.Parse<long>(),
                BrandBoxIds = BrandBoxIds.Parse<long>(),
                Email = Email,
                Phone = Phone,
                BrandIds = BrandIds.Parse<long>(),
                CampaignIds = CampaignIds.Parse<long>(),
                FirstName = FirstName,
                IntegrationIds = IntegrationIds.Parse<long>(),
                OfferIds = OfferIds.Parse<long>(),
                LastName = LastName,
                DateType = DateType ?? DateTimeType.RegistrationDate
            };
        }

    }
}
