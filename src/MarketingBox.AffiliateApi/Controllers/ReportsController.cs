using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Reports;
using MarketingBox.AffiliateApi.Pagination;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;

namespace MarketingBox.AffiliateApi.Controllers
{
    [ApiController]
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
            request.TenantId = tenantId;
            var response = await _reportService.SearchAsync(_mapper.Map<ReportSearchRequest>(request));

            if (response.Error != null)
            {
                ModelState.AddModelError("", response.Error.Message);

                return BadRequest(ModelState);
            }

            if (response.Reports == null)
                return NotFound();

            return Ok(
                response.Reports.Select(x => _mapper.Map<ReportModel>(x))
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }
    }
}