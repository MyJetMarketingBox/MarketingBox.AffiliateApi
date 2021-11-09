using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Reports;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Route("/api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = AuthorizationPolicies.AffiliateManagerAndHigher)]
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<ReportModel, long>), StatusCodes.Status200OK)]

        public async Task<ActionResult<Paginated<ReportModel, long>>> SearchAsync(
            [FromQuery] Models.Reports.Requests.ReportSearchRequest request)
        {
            if (request.Limit < 1 || request.Limit > 1000)
            {
                ModelState.AddModelError($"{nameof(request.Limit)}", "Should be in the range 1..1000");

                return BadRequest();
            }

            var tenantId = this.GetTenantId();
            var response = await _reportService.SearchAsync(new ReportSearchRequest()
            {
                Asc = request.Order == PaginationOrder.Asc,
                Cursor = request.Cursor,
                FromDate = DateTime.SpecifyKind(request.FromDate, DateTimeKind.Utc),
                ToDate = DateTime.SpecifyKind(request.ToDate, DateTimeKind.Utc),
                Take = request.Limit,
                TenantId = tenantId
            });

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Reports == null)
                return NotFound();

            return Ok(
                response.Reports.Select(x => new ReportModel()
                {
                    AffiliateId = x.AffiliateId,
                    Cr = x.Cr,
                    FtdCount = x.FtdCount,
                    RegistrationsCount = x.RegistrationCount,
                    Payout = x.Payout,
                    Revenue = x.Revenue,
                })
                    .ToArray()
                    .Paginate(request, Url, x => x.AffiliateId));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Authorize(Policy = AuthorizationPolicies.AffiliateAndHigher)]
        [HttpGet("by-days")]
        [ProducesResponseType(typeof(ItemsContainer<ReportByDaysModel>), StatusCodes.Status200OK)]

        public async Task<ActionResult<ItemsContainer<ReportByDaysModel>>> SearchByDaysAsync()
        {
            var tenantId = this.GetTenantId();
            var now = DateTime.UtcNow;
            var days = System.DateTime.DaysInMonth(now.Year, now.Month);
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0);
            var endOfMonth = new DateTime(now.Year, now.Month, days, 23, 59, 59);
            var response = await _reportService.SearchByDayAsync(new ReportByDaySearchRequest()
            {
                Asc = true,
                Cursor = null,
                FromDate = startOfMonth,
                ToDate = endOfMonth,
                Take = days,
                TenantId = tenantId
            });

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Reports == null)
                return NotFound();

            return Ok(new ItemsContainer<ReportByDaysModel>()
            {
                Items = response.Reports.Select(x => new ReportByDaysModel()
                    {
                        CreatedAt = x.CreatedAt,
                        FtdCount = x.FtdCount,
                        RegistrationsCount = x.RegistrationCount,
                    })
                    .ToArray()
            });
        }
    }
}