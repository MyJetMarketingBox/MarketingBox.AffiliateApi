using System.Linq;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Reports;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportsController(
            IReportService reportService,
            IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(Paginated<ReportModel, long?>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Paginated<ReportModel, long?>>> SearchAsync(
            [FromQuery] Models.Reports.Requests.ReportSearchRequest request)
        {
            var tenantId = this.GetTenantId();
            request.TenantId = tenantId;
            var response = await _reportService.SearchAsync(_mapper.Map<ReportSearchRequest>(request));

            return this.ProcessResult(response,
                response.Data?.Select(_mapper.Map<ReportModel>)
                    .ToArray()
                    .Paginate(request, Url, response.Total ?? default, x => x.Id));
        }
    }
}