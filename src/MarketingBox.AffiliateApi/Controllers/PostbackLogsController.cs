using System;
using System.Linq;
using AutoMapper;
using MarketingBox.Postback.Service.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.AffiliateApi.Models.PostbackLogs.Requests;
using MarketingBox.Postback.Service.Domain.Models.Requests;
using MarketingBox.Sdk.Common.Extensions;
using MarketingBox.Sdk.Common.Models.RestApi;
using MarketingBox.Sdk.Common.Models.RestApi.Pagination;
using EventReferenceLog = MarketingBox.AffiliateApi.Models.PostbackLogs.EventReferenceLog;

namespace MarketingBox.AffiliateApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]/")]
    public class PostbackLogsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PostbackLogsController> _logger;
        private readonly IEventReferenceLogService _eventReferenceLogService;

        public PostbackLogsController(
            IEventReferenceLogService eventReferenceLogService,
            IMapper mapper,
            ILogger<PostbackLogsController> logger)
        {
            _eventReferenceLogService = eventReferenceLogService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Paginated<EventReferenceLog, long?>>> GetLogs(
            [FromQuery] SearchPostbackLogsRequest paginationLogsRequest)
        {
            var tenantId = this.GetTenantId();
            var request = new FilterLogsRequest()
            {
                Asc = paginationLogsRequest.Order == PaginationOrder.Asc,
                Cursor = paginationLogsRequest.Cursor,
                Take = paginationLogsRequest.Limit,
                AffiliateName = paginationLogsRequest.AffiliateName,
                EventType = paginationLogsRequest.EventType,
                ToDate = paginationLogsRequest.ToDate,
                FromDate = paginationLogsRequest.FromDate,
                HttpQueryType = paginationLogsRequest.HttpQueryType,
                ResponseStatus = paginationLogsRequest.ResponseStatus,
                RegistrationUId = paginationLogsRequest.RegistrationUId,
                AffiliateIds = paginationLogsRequest.AffiliateIds.Parse<long>(),
                TenantId = tenantId
            };
            var response = await _eventReferenceLogService.SearchAsync(request);
            
            return this.ProcessResult(
                response,
                (response.Data?
                    .Select(_mapper.Map<EventReferenceLog>)
                    .ToArray() ?? Array.Empty<EventReferenceLog>())
                    .Paginate(paginationLogsRequest, Url, response.Total ?? default, x => x.Id));
        }
    }
}