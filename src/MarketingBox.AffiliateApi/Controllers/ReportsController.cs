using System.Collections.Generic;
using System.Linq;
using MarketingBox.AffiliateApi.Extensions;
using MarketingBox.AffiliateApi.Models.Reports;
using MarketingBox.Reporting.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using MarketingBox.AffiliateApi.Authorization;
using MarketingBox.Reporting.Service.Domain.Models.Reports.Requests;
using MarketingBox.Sdk.Common.Exceptions;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using ValidationError = MarketingBox.Sdk.Common.Models.ValidationError;

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

        public async Task<ActionResult<Paginated<ReportModel, long?>>> SearchAsync(
            [FromQuery] Models.Reports.Requests.ReportSearchRequest request)
        {
            if (request.Limit is < 1 or > 1000)
            {
                throw new ApiException(new Error
                {
                    ErrorMessage = BadRequestException.DefaultErrorMessage,
                    ValidationErrors = new List<ValidationError>
                    {
                        new ()
                        {
                            ErrorMessage = "Should be in the range 1..1000",
                            ParameterName = nameof(request.Limit)
                        }
                    }
                });
            }

            var tenantId = this.GetTenantId();
            request.TenantId = tenantId;
            var response = await _reportService.SearchAsync(_mapper.Map<ReportSearchRequest>(request));

            return this.ProcessResult(response,
                response.Data?.Select(x => _mapper.Map<ReportModel>(x))
                    .ToArray()
                    .Paginate(request, Url, x => x.Id));
        }
    }
}