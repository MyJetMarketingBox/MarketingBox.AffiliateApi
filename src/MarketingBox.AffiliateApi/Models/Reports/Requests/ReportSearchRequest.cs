﻿using System;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Domain.Models.Reports;

namespace MarketingBox.AffiliateApi.Models.Reports.Requests
{
    public class ReportSearchRequest : PaginationRequest<long?>
    {
        public long? AffiliateId { get; set; }
        public string Country { get; set; }
        public long? BrandId { get; set; }
        public string Offer { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ReportType ReportType { get; set; }
        internal string TenantId { get; set; }
    }
}
